using BattleRoyal_RPG.Competences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    public class Pretre : Personnage
    {
        private Random _random = new Random();
        public Pretre(string Name ) : base(Name)
        {
            Competences[0].Type = TypeAttaque.Sacre;
            Competences.Add(new Soin());
        }


        public override async Task ExecuterStrategie()
        {
            
            Personnage cible = ChoisirCible();

            var competenceSoin = Competences.FirstOrDefault(c => c is Soin && c.EstDisponible);
            var competenceAttaque = Competences.FirstOrDefault(c => c.EstDisponible && c is AttaqueBase);

            // Décidez quand utiliser le soin
            if (competenceSoin != null)
            {
                /*
                // Supposons que vous ayez une liste de tous les alliés
                var allieBlesse = GetAllies().OrderBy(a => a.Vie).FirstOrDefault();

                if (allieBlesse != null && allieBlesse.Vie <= 50)  // si un allié a 50 points de vie ou moins
                {
                    await competenceSoin.Utiliser(this, allieBlesse);
                    return;
                }
                */

                // Si le prêtre est gravement blessé
                if (Vie <= 50)  // si le prêtre a 30 points de vie ou moins
                {
                    await competenceSoin.Utiliser(this, this);
                    return;
                }
  
            }

            // Si un MortVivant est une cible viable
            if (competenceSoin != null && cible.TypeDuPersonnage == TypePersonnage.MortVivant && cible.Vie <= 100)
            {
                await competenceSoin.Utiliser(this, cible);
                return;
            }

            // Si rien de ce qui précède ne s'applique, utilisez l'attaque de base
            if (competenceAttaque != null )
            {
                await competenceAttaque.Utiliser(this, cible);
            }

        }

        // Méthode pour choisir la cible
        private Personnage ChoisirCible()
        {
            return GetEnemies();
        }
        private Personnage GetEnemies()
        {
            // Trouver tous les MortVivants encore en vie.
            var mortVivants = BattleArena.Participants.Where(p => p.Vie > 0 && p.TypeDuPersonnage == TypePersonnage.MortVivant).ToList();

            if (mortVivants.Any())
            {
                List<Personnage> ciblesMortVivant = new List<Personnage>();

                foreach (var participant in mortVivants)
                {
                    if (!participant.EstMort && participant.TypeDuPersonnage == TypePersonnage.MortVivant)
                    {
                        ciblesMortVivant.Add(participant);
                    }
                }

                int indexAleatoireMortVivant = _random.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
                return ciblesMortVivant[indexAleatoireMortVivant];

            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Les MortsVivants ont été décimé");
            Console.ResetColor();

            int indexAleatoire ; // Sélectionner un index aléatoire.
            do
            {
                indexAleatoire = _random.Next(BattleArena.Participants.Count); // Sélectionner un index aléatoire.
            } while (BattleArena.Participants[indexAleatoire] == this || BattleArena.Participants[indexAleatoire].EstMort);

            // Si aucun MortVivant n'est trouvé, tous les autres personnages sont les ennemis.
            return BattleArena.Participants[indexAleatoire];
        }


    }
}
