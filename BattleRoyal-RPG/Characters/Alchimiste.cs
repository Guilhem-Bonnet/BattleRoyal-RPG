using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Characters
{
    internal class Alchimiste : Personnage
    {
        public Alchimiste(string Name) : base(Name)
        {
            Competences[0] = new JetDePotion((FightService)_fightservice);
            Competences.Add(new PotionChangeLife());
            Defense = 2;
            
        }

        public override async Task Strategie()
        {
            Personnage cible = ChoisirCible();
            Personnage cibleChangeLife = ChoisirCibleChangeLife();
            if (cible == null)
            {
                return;
            }

            var competencePotionChangeLife = Competences.FirstOrDefault(c => c is PotionChangeLife && c.EstDisponible);
            var competenceAttackBase = Competences.FirstOrDefault(c => c.EstDisponible && c is JetDePotion);

            // Décidez quand utiliser le soin
            if (competencePotionChangeLife != null && cibleChangeLife != null && Life <= MaxLife*0.8) // Si la compétence est disponible et que la cible est valide et que la Life est inférieur à 80%
            {
                await competencePotionChangeLife.Utiliser(this, cibleChangeLife);
                return;
            }
            

            // Si rien de ce qui précède ne s'applique, utilisez l'Attack de base
            if (competenceAttackBase != null)
            {
                await competenceAttackBase.Utiliser(this, cible);
            }
        }
        private Personnage ChoisirCible()
        {
            return GetEnemies();
        }
        private Personnage ChoisirCibleChangeLife()
        {
            Personnage personnageCible = this;
            foreach (var participant in BattleArena.Participants)
            {
                if (!participant.IsDead && participant != this)
                {
                    if (participant.Life > personnageCible.Life)
                    {
                        personnageCible = participant;
                    }
                    
                }
            }
            if (personnageCible == this)
            {
                personnageCible = null;
            }
            return personnageCible;
        }
        private Personnage GetEnemies()
        {
            // Trouver tous les MortVivants encore en Life.
            var mortVivants = BattleArena.Participants.Where(p => p.Life > 0 && p.TypeDuPersonnage == TypePersonnage.MortVivant).ToList();

            if (mortVivants.Any())
            {
                List<Personnage> ciblesMortVivant = new List<Personnage>();

                foreach (var participant in mortVivants)
                {
                    if (!participant.IsDead && participant.TypeDuPersonnage == TypePersonnage.MortVivant)
                    {
                        ciblesMortVivant.Add(participant);
                    }
                }

                int indexAleatoireMortVivant = _rand.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
                return ciblesMortVivant[indexAleatoireMortVivant];

            }
            if (BattleArena.Participants.Count>1)
            {
                Personnage cibleAleatoire; // Sélectionner un index aléatoire.
                List<Personnage> cibles = new List<Personnage>();
                foreach (var participant in BattleArena.Participants)
                {
                    if (!participant.IsDead && participant != this)
                    {
                        cibles.Add(participant);
                    }
                }
                if (cibles.Count == 0)
                {
                    return null;
                }

                cibleAleatoire = cibles[_rand.Next(cibles.Count)]; // Sélectionner un index aléatoire.


                // Si aucun MortVivant n'est trouvé, tous les autres personnages sont les ennemis.
                return cibleAleatoire;
            }
            else
            {
                return null;
            }
      
        }
    }
}
