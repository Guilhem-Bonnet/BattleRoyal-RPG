using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Observeur;
using System.Diagnostics.Metrics;

namespace BattleRoyal_RPG
{
    public abstract class Personnage : IPersonnage
    {
        public string Nom { get; set; }
        public int VieMax { get; set; } = 100;
        private int _vie ;
        public int Attaque { get; set; } = 5;
        public int Defense { get; set; } = 10;
        public virtual bool EstAttaquable => !EstMort;  // Par défaut, un personnage est attaquable s'il n'est pas mort.
        public virtual bool EstMort => Vie <= 0;  // Par défaut, un personnage est mort si sa vie est <= 0.
        private bool? _estMangeable; // Nullable pour déterminer si elle a été explicitement définie.
        protected Random _random = new Random();
        public virtual bool EstMangeable
        {
            get => _estMangeable ?? EstMort;  // Si _estMangeable n'a pas été défini, il retournera EstMort.
            set => _estMangeable = value;
        }
        public TypePersonnage TypeDuPersonnage { get; set; } = TypePersonnage.Humain;
        public int Vie
                {
                    get { return _vie; }
                    set
                    {
                        if (_vie > 0 && value <= 0)
                        {
                            var message = new Message();
                            message.AddSegment($"{Nom} est mort !", ConsoleColor.Red);
                            notifier.AddMessageToQueue(message);
                            new Decorateur.EstMort(this);
                            
                        }
                        _vie = value;
                    }
                }

        private List<Etat> Etats { get; } = new List<Etat>();

        public List<ICompetence> Competences { get ; set ; } = new List<ICompetence>();
        
        protected Random _rand = new Random();
        const int FACE_DE = 20;
        // Champ notifier
        public MessageNotifier notifier = MessageNotifier.Instance;
        public Personnage etatOriginal;
  
        public Personnage(string nom)
        {
            etatOriginal = this;
            Nom = nom;
            Competences.Add(new AttaqueBase());
            _vie = VieMax;
            

        }
        
        public void AttaquerBase(Personnage cible)
        {
            Competences[0].Utiliser(this, cible);
        }
        public virtual int CalculerDommage(ResultatDe resultatAttaque, ResultatDe resultatDefense, TypeAttaque typeAttaque, IPersonnage cible, int attaque = -1)
        {

            var message = new Message();
                message.AddSegment($"{Nom} à {Vie}pv et {Defense}def \n");

            int baseDommage=0;
            if (attaque < 0) attaque = Attaque;

            int dommageAttaque = ResultaAttaque(resultatAttaque, attaque);
            int resultDefense = ResultatDefense(resultatDefense,cible);

            // Cas spécifiques
            if (resultatAttaque == ResultatDe.RéussiteCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                baseDommage = dommageAttaque - resultDefense;

                message.AddSegment($"Choc épique ! ", ConsoleColor.Magenta)
                   .AddSegment($"{Nom}", ConsoleColor.Red)
                   .AddSegment($" et ", ConsoleColor.Magenta)
                   .AddSegment($"{cible.Nom}", ConsoleColor.Red)
                   .AddSegment($" démontrent une maîtrise incroyable !", ConsoleColor.Magenta);
                notifier.AddMessageToQueue(message);


            }
            else if (resultatAttaque == ResultatDe.EchecCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                baseDommage = cible.Attaque * 2; // La défense fait des dégâts doublés à l'attaquant en tant que contre-attaque
                message.AddSegment($"{Nom} tente une attaque risquée", ConsoleColor.Red)
                    .AddSegment($" mais ", ConsoleColor.Magenta)
                    .AddSegment($"{cible.Nom}", ConsoleColor.Red)
                    .AddSegment($" retourne brillamment la situation avec une contre-attaque !", ConsoleColor.Yellow);
 
                baseDommage = CalculVulnerabilites(baseDommage, typeAttaque, cible, message);

                message.AddSegment($"{ cible.Nom }, ConsoleColor.Red)")
                    .AddSegment($"inflige")
                    .AddSegment($" {baseDommage} dégâts!", ConsoleColor.Magenta)
                    .AddSegment($" à ")
                    .AddSegment($"{Nom} ", ConsoleColor.Blue);

                notifier.AddMessageToQueue(message);
                InfligerDommages(baseDommage, this);



                return 0;
            }
            else
            {
                baseDommage = dommageAttaque - resultDefense;
            }

            if (baseDommage < 0) baseDommage = 0;

            message.AddSegment($"{Nom}", ConsoleColor.Blue)
                    .AddSegment(" Attaque: ")
                    .AddSegment($"{resultatAttaque} ", ConsoleColor.Cyan)
                    .AddSegment("contre | ")
                    .AddSegment($"{cible.Nom}", ConsoleColor.Blue)
                    .AddSegment(" Defense:")
                    .AddSegment($"{resultatDefense}\n ", ConsoleColor.Cyan)
                    .AddSegment($"Type de l'attaque : ");
                    if (typeAttaque == TypeAttaque.Sacre)
                    {
                         message.AddSegment($"{typeAttaque}\n", ConsoleColor.Yellow);
                    }
                    else
                    {
                        message.AddSegment($"{typeAttaque}\n");
                    }
             message.AddSegment( "Attaque Base:")
                    .AddSegment($"{attaque}\n", ConsoleColor.Blue)
                    .AddSegment("Attaque: ")
                    .AddSegment($"{dommageAttaque}\n", ConsoleColor.Green)
                    .AddSegment("Défense: ")
                    .AddSegment($"{resultDefense}\n", ConsoleColor.Green);
                

            // Multiplier les dégâts si l'attaque est sacrée et que la cible est un mort-vivant
            baseDommage = CalculVulnerabilites(baseDommage, typeAttaque, cible,message);
            
            notifier.AddMessageToQueue(message);
            return baseDommage;
        }

        public void InfligerDommages(int dommages,IPersonnage cible)
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
                    return dommage;
            }

            
        }
      

        private int ResultatDefense(ResultatDe resultatDefense, IPersonnage defenseur)
        {
              switch (resultatDefense)
            {
                case ResultatDe.EchecCritique:
                    return 0;
                case ResultatDe.Echec:
                    return 0;
                case ResultatDe.Neutre:
                    return defenseur.Defense;
                case ResultatDe.Réussite:
                    return (int)(defenseur.Defense * 1.5);
                case ResultatDe.RéussiteCritique:
                    return defenseur.Defense * 2;
                default:
                    return defenseur.Defense;
            }
        }
        private int ResultaAttaque(ResultatDe resultatAttaque, int attaque = -1)
        {

            switch (resultatAttaque)
            {
                case ResultatDe.EchecCritique:
                    return 0;
                case ResultatDe.Echec:
                    return 0;
                case ResultatDe.Neutre:
                    return attaque;
                case ResultatDe.Réussite:
                    return (int)(attaque * 1.5);
                case ResultatDe.RéussiteCritique:
                    return attaque * 2;
                default:
                    return attaque;
            }
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

        public void AjouterEtat(Etat etat)
        {
            Etats.Add(etat);
            etat.Debut();
        }

        public void RetirerEtat<T>() where T : Etat
        {
            var etat = Etats.OfType<T>().FirstOrDefault();
            if (etat != null)
            {
                etat.Fin();
                Etats.Remove(etat);
            }
        }

        public bool AEtat<T>() where T : Etat
        {
            return Etats.OfType<T>().Any();
        }

        public void AppliquerEtats()
        {
            foreach (var etat in Etats)
            {
                etat.Appliquer();
            }
        }

        public virtual async Task ExecuterStrategie() { }



    }
}
