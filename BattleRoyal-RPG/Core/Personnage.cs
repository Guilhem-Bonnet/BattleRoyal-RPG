using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Observeur;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Utilities;

namespace BattleRoyal_RPG.Core
{
    public abstract class Personnage : IPersonnage
    {
        #region Champs
        private int _vie;
        private bool? _estMangeable;
        private readonly object etatLock = new object();
        protected Random _rand = new Random();
        const int FACE_DE = 20;
        public static readonly MessageNotifier notifier = MessageNotifier.Instance;

        private const int EchecCritiqueMultiplier = 0;
        private const int NeutreMultiplier = 1;
        private const int ReussiteMultiplier = 2;
        #endregion

        #region Propriétés
        public string Nom { get; set; }
        public int VieMax { get; set; } = 100;
        public int Attaque { get; set; } = 5;
        public int Defense { get; set; } = 10;
        public TypePersonnage TypeDuPersonnage { get; set; } = TypePersonnage.Humain;
        public Personnage etatOriginal;

        public virtual bool EstAttaquable => !EstMort;
        public virtual bool EstMort => Vie <= 0;
        public virtual bool EstMangeable
        {
            get => _estMangeable ?? EstMort;
            set => _estMangeable = value;
        }

        public int Vie
        {
            get { return _vie; }
            set
            {
                _vie = value;
            }
        }
        #endregion

        #region Collections
        public List<Etat> Etats { get; } = new List<Etat>();
        public List<ICompetence> Competences { get; set; } = new List<ICompetence>();
        #endregion

        #region Constructeurs
        public Personnage(string nom)
        {
            etatOriginal = this;
            Nom = nom;
            Competences.Add(new AttaqueBase());
            _vie = VieMax;


        }
        #endregion

        #region Méthodes
        public void AttaquerBase(Personnage cible)
        {
            Competences[0].Utiliser(this, cible);
        }

        public virtual int CalculerDommage(ResultatDe resultatAttaque, ResultatDe resultatDefense, TypeAttaque typeAttaque, IPersonnage cible, int attaque = -1)
        {
            if (attaque < 0) attaque = Attaque;

            var messageBuilder = new MessageBuilder();
            var mainMessage = messageBuilder.LifeMessage(Nom, Vie, Defense);


            var (baseDommage, casSpecifiquesMessage) = TraiterCasSpecifiquesEtConstruireMessage(resultatAttaque, resultatDefense, typeAttaque, cible, attaque);
            mainMessage.AddSegmentsFrom(casSpecifiquesMessage);

            var (adjustedDommage, vulnerabilitesMessage) = CalculerVulnerabilitesEtConstruireMessage(baseDommage, typeAttaque, cible);
            mainMessage.AddSegmentsFrom(vulnerabilitesMessage);

            notifier.AddMessageToQueue(mainMessage);

            return adjustedDommage;
        }

        private Message ConstruireMessagePrincipal()
        {
            var message = new Message();
            message.AddSegment($"{Nom} à {Vie}pv et {Defense}def \n");
            return message;
        }

        private (int, Message) TraiterCasSpecifiquesEtConstruireMessage(ResultatDe resultatAttaque, ResultatDe resultatDefense, TypeAttaque typeAttaque, IPersonnage cible, int attaque)
        {
            var message = new Message();
            int dommageAttaque = ResultatAttaque(resultatAttaque, attaque);
            int resultDefense = ResultatDefense(resultatDefense, cible);
            int baseDommage = dommageAttaque - resultDefense;

            if (resultatAttaque == ResultatDe.RéussiteCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                message.AddSegment($"Choc épique ! ", ConsoleColor.Magenta)
                    .AddSegment($"{Nom}", ConsoleColor.Red)
                    .AddSegment($" et ", ConsoleColor.Magenta)
                    .AddSegment($"{cible.Nom}", ConsoleColor.Red)
                    .AddSegment($" démontrent une maîtrise incroyable !", ConsoleColor.Magenta);
            }
            else if (resultatAttaque == ResultatDe.EchecCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                baseDommage = cible.Attaque * 2;
                message.AddSegment($"{Nom} tente une attaque risquée", ConsoleColor.Red)
                    .AddSegment($" mais ", ConsoleColor.Magenta)
                    .AddSegment($"{cible.Nom}", ConsoleColor.Red)
                    .AddSegment($" retourne brillamment la situation avec une contre-attaque !", ConsoleColor.Yellow);

                baseDommage = CalculVulnerabilites(baseDommage, typeAttaque, cible, message);

                message.AddSegment($"{cible.Nom} ", ConsoleColor.Red)
                    .AddSegment($"inflige ")
                    .AddSegment($"{baseDommage} dégâts! ", ConsoleColor.Magenta)
                    .AddSegment($"à ")
                    .AddSegment($"{Nom} ", ConsoleColor.Blue);

                InfligerDommages(baseDommage, this);
                return (0, message);  // Si cet état est atteint, le retour se fait ici
            }

            if (baseDommage < 0) baseDommage = 0;

            ConstruireMessageCombat(message, resultatAttaque, resultatDefense, typeAttaque, cible, attaque, dommageAttaque, resultDefense);

            return (baseDommage, message);
        }

        private void ConstruireMessageCombat(Message message, ResultatDe resultatAttaque, ResultatDe resultatDefense, TypeAttaque typeAttaque, IPersonnage cible, int attaque, int dommageAttaque, int resultDefense)
        {
            message.AddSegment($"{Nom}", ConsoleColor.Blue)
                .AddSegment(" Attaque: ").AddSegment($"{resultatAttaque} ", ConsoleColor.Cyan)
                .AddSegment("contre | ")
                .AddSegment($"{cible.Nom}", ConsoleColor.Blue)
                .AddSegment(" Defense:").AddSegment($"{resultatDefense}\n ", ConsoleColor.Cyan)
                .AddSegment($"Type de l'attaque : ");

            if (typeAttaque == TypeAttaque.Sacre)
            {
                message.AddSegment($"{typeAttaque}\n", ConsoleColor.Yellow);
            }
            else
            {
                message.AddSegment($"{typeAttaque}\n");
            }

            message.AddSegment("Attaque Base:")
                .AddSegment($"{attaque}\n", ConsoleColor.Blue)
                .AddSegment("Attaque: ")
                .AddSegment($"{dommageAttaque}\n", ConsoleColor.Green)
                .AddSegment("Défense: ")
                .AddSegment($"{resultDefense}\n", ConsoleColor.Green);
        }

        private (int, Message) CalculerVulnerabilitesEtConstruireMessage(int baseDommage, TypeAttaque typeAttaque, IPersonnage cible)
        {
            var message = new Message();
            int adjustedDommage = CalculVulnerabilites(baseDommage, typeAttaque, cible, message);
            return (adjustedDommage, message);
        }

        public void InfligerDommages(int dommages, IPersonnage cible)
        {
            var message = new Message();
            message.AddSegment($"{cible.Nom}", ConsoleColor.Blue)
                .AddSegment($" perd ")
                .AddSegment($"{dommages}pv \n", ConsoleColor.Red)
                .AddSegment($"{cible.Nom} new vie: ");

            if (cible.Vie - dommages <= 0)
            {
                message.AddSegment($"0", ConsoleColor.Red);
            }
            else
            {
                message.AddSegment($"{cible.Vie - dommages}", ConsoleColor.Green);
            }
            notifier.AddMessageToQueue(message);
            cible.Vie -= dommages;
        }

        private int CalculVulnerabilites(int dommage, TypeAttaque typeAttaque, IPersonnage cible, Message message = null)
        {
            switch (typeAttaque)
            {
                case TypeAttaque.Sacre:
                    if (cible.TypeDuPersonnage == TypePersonnage.MortVivant)
                    {
                        dommage += Attaque;

                        if (message != null)
                            message.AddSegment($"C'est super efficace ! Dégâts: ", ConsoleColor.Yellow)
                                   .AddSegment($"{dommage}", ConsoleColor.Green);

                    }
                    return dommage;
                case TypeAttaque.Normal:
                    return dommage;
                default:
                    Debug.Assert(false, "Type d'attaque inconnu.");
                    return dommage;
            }


        }

        private int AppliquerMultiplier(int baseValue, int multiplier)
        {
            return baseValue * multiplier;
        }
        private int ResultatDefense(ResultatDe resultatDefense, IPersonnage defenseur)
        {
            int multiplier = resultatDefense switch
            {
                ResultatDe.EchecCritique => EchecCritiqueMultiplier,
                ResultatDe.Echec => EchecCritiqueMultiplier,
                ResultatDe.Neutre => NeutreMultiplier,
                ResultatDe.Réussite => (int)(NeutreMultiplier * 1.5),
                ResultatDe.RéussiteCritique => ReussiteMultiplier,
                _ => defenseur.Defense,
            };

            return AppliquerMultiplier(defenseur.Defense, multiplier);
        }

        private int ResultatAttaque(ResultatDe resultatAttaque, int attaque = -1)
        {
            int multiplier = resultatAttaque switch
            {
                ResultatDe.EchecCritique => EchecCritiqueMultiplier,
                ResultatDe.Echec => EchecCritiqueMultiplier,
                ResultatDe.Neutre => NeutreMultiplier,
                ResultatDe.Réussite => (int)(NeutreMultiplier * 1.5),
                ResultatDe.RéussiteCritique => ReussiteMultiplier,
                _ => Attaque,
            };
            return AppliquerMultiplier(attaque, multiplier);
        }

        public ResultatDe LancerDe()
        {
            int resultat = _rand.Next(1, FACE_DE + 1);

            if (resultat == 1)
                return ResultatDe.EchecCritique;
            else if (resultat == 20)
                return ResultatDe.RéussiteCritique;
            else if (resultat <= 5)
                return ResultatDe.Echec;
            else if (resultat >= 16)
                return ResultatDe.Réussite;
            else
                return ResultatDe.Neutre;
        }

        private void AjouterEtat(Etat etat)
        {
            Etats.Add(etat);
            _ = etat.Appliquer();
        }
        public void AppliquerOuCumulerEtat(Etat etat)
        {
            lock (etatLock) // Lock sur un objet dédié pour éviter les deadlocks
            {
                var etatType = etat.GetType();
                var etatExistant = Etats.FirstOrDefault(e => e.GetType() == etatType);

                if (etatExistant != null)
                {
                    etatExistant.Cumuler(); // Si un état du même type existe déjà, cumuler cet état.
                }
                else
                {
                    AjouterEtat(etat); // Sinon, ajouter le nouvel état à la liste des états du personnage.
                }
            }
        }

        public void RetirerEtat<T>() where T : Etat
        {
            var etat = Etats.OfType<T>().FirstOrDefault();
            if (etat != null)
            {
                _ = etat.Fin();
            }
        }

        public bool AEtat<T>() where T : Etat
        {
            return Etats.OfType<T>().Any();
        }


        public async Task ExecuterStrategie()
        {

            while (Vie > 0 && !BattleArena.EndBattle)
            {
                await Strategie();
            }

        }
        public virtual async Task Strategie()
        {

        }

        #endregion

    }
}
