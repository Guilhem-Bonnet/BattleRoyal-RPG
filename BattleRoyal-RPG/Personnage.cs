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
        public virtual int CalculerDommage( ResultatDe resultatAttaque, ResultatDe resultatDefense, TypeAttaque typeAttaque, IPersonnage cible, int attaque = -1)
        {
            int baseDommage;
            if (attaque == -1) attaque = Attaque;

            switch (resultatAttaque)
            {
                case ResultatDe.EchecCritique when resultatDefense == ResultatDe.RéussiteCritique:
                    // L'attaquant subit une contre-attaque
                    Vie -= cible.Defense * 2;
                    Console.WriteLine($"{Nom} subit une contre-attaque !");
                    Console.WriteLine($"{Nom} perd {Defense * 2} points de vie !");
                    baseDommage = 0; // Pas de dommage à la cible
                    break;
                case ResultatDe.EchecCritique:
                case ResultatDe.Echec:
                    baseDommage = 0; // Pas de dommage à la cible
                    break;
                case ResultatDe.Neutre when resultatDefense == ResultatDe.Neutre:
                    baseDommage = attaque - cible.Defense; // Dommage basique diminué par la défense
                    break;
                case ResultatDe.RéussiteCritique:
                    baseDommage = attaque * 2; // Double dommage
                    break;
                default:
                    baseDommage = attaque;
                    break;
            }
            if (baseDommage < 0) baseDommage = 0;
            Console.WriteLine($"{Nom} Attaque:{resultatAttaque} contre | {cible.Nom} Defense:{resultatDefense}");
            Console.WriteLine($"type de l'attaque :{typeAttaque}");
            // Multiplier les dégâts si l'attaque est sacrée et que la cible est un mort-vivant
            if (typeAttaque == TypeAttaque.Sacre && cible.TypeDuPersonnage == TypePersonnage.MortVivant)
            {
                baseDommage *= 2;
                Console.WriteLine($"C'est super efficace ! dégats: {baseDommage}");
            }

            return baseDommage;
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

        public virtual async Task ExecuterStrategie() { 
        }



    }
}
