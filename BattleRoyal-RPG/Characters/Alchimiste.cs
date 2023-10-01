using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Characters
{
    internal class Alchimiste : Personnage
    {
        public Alchimiste(string nom) : base(nom)
        {
            Competences[0] = new JetDePotion();
            Competences.Add(new PotionChangeVie());
            Defense = 2;
            
        }

        public override async Task Strategie()
        {
            Personnage cible = ChoisirCible();
            Personnage cibleChangeVie = ChoisirCibleChangeVie();
            if (cible == null)
            {
                return;
            }

            var competencePotionChangeVie = Competences.FirstOrDefault(c => c is PotionChangeVie && c.EstDisponible);
            var competenceAttaqueBase = Competences.FirstOrDefault(c => c.EstDisponible && c is JetDePotion);

            // Décidez quand utiliser le soin
            if (competencePotionChangeVie != null && cibleChangeVie != null && Vie <= VieMax*0.8) // Si la compétence est disponible et que la cible est valide et que la vie est inférieur à 80%
            {
                await competencePotionChangeVie.Utiliser(this, cibleChangeVie);
                return;
            }
            

            // Si rien de ce qui précède ne s'applique, utilisez l'attaque de base
            if (competenceAttaqueBase != null)
            {
                await competenceAttaqueBase.Utiliser(this, cible);
            }
        }
        private Personnage ChoisirCible()
        {
            return GetEnemies();
        }
        private Personnage ChoisirCibleChangeVie()
        {
            Personnage personnageCible = this;
            foreach (var participant in BattleArena.Participants)
            {
                if (!participant.EstMort && participant != this)
                {
                    if (participant.Vie > personnageCible.Vie)
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

                int indexAleatoireMortVivant = _rand.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
                return ciblesMortVivant[indexAleatoireMortVivant];

            }
            if (BattleArena.Participants.Count>1)
            {
                Personnage cibleAleatoire; // Sélectionner un index aléatoire.
                List<Personnage> cibles = new List<Personnage>();
                foreach (var participant in BattleArena.Participants)
                {
                    if (!participant.EstMort && participant != this)
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
