using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Competences;

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
        public virtual bool EstMangeable { get; set; }=false; // Par défaut, un personnage n'est pas mangeable.
        public TypePersonnage TypeDuPersonnage { get; set; } = TypePersonnage.Humain;
        public int Vie
                {
                    get { return _vie; }
                    set
                    {
                        if (_vie > 0 && value <= 0)
                        {
                            new Decorateur.EstMort(this);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine($"{Nom} est mort !");
                            Console.ResetColor();
                        }
                        _vie = value;
                    }
                }
        private List<Etat> Etats { get; } = new List<Etat>();

        public List<ICompetence> Competences { get ; set ; } = new List<ICompetence>();
        
        protected Random _rand = new Random();
        const int FACE_DE = 20;
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
            int baseDommage=0;
            if (attaque < 0) attaque = Attaque;

            int dommageAttaque = ResultaAttaque(resultatAttaque, attaque);
            int resultDefense = ResultatDefense(resultatDefense,cible);

            // Cas spécifiques
            if (resultatAttaque == ResultatDe.RéussiteCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                baseDommage = dommageAttaque - resultDefense;

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"Choc épique ! {Nom} et {cible.Nom} démontrent une maîtrise incroyable !");
            }
            else if (resultatAttaque == ResultatDe.EchecCritique && resultatDefense == ResultatDe.RéussiteCritique)
            {
                baseDommage = -cible.Attaque * 2; // La défense fait des dégâts doublés à l'attaquant en tant que contre-attaque
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Nom} tente une attaque risquée, mais {cible.Nom} retourne brillamment la situation avec une contre-attaque dévastatrice !");
                
                Vie += baseDommage; // On ajoute les dégâts négatifs à la vie
                return 0;
            }
            else
            {
                baseDommage = dommageAttaque - resultDefense;
            }
            if (baseDommage < 0) baseDommage = 0;
            

            Console.ResetColor();
            Console.WriteLine($"{Nom} Attaque:{resultatAttaque} contre | {cible.Nom} Defense:{resultatDefense}");
            Console.WriteLine($"Type de l'attaque : {typeAttaque}");

            Console.Write("Attaque Base: ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{attaque}");
            Console.ResetColor();

            Console.Write("Attaque: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{dommageAttaque}");
            Console.ResetColor();

            Console.Write("Défense: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{resultDefense}");
            Console.ResetColor();

            // Multiplier les dégâts si l'attaque est sacrée et que la cible est un mort-vivant
            if (typeAttaque == TypeAttaque.Sacre && cible.TypeDuPersonnage == TypePersonnage.MortVivant)
            {
                baseDommage += attaque;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"C'est super efficace ! Dégâts: {baseDommage}");
                Console.ResetColor();
            }
            // Mettre en couleur les dégâts et la défense
            Console.Write("Dégâts infligés: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{baseDommage}");
            Console.ResetColor();

            return baseDommage;
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
