using BattleRoyal_RPG.Competences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    internal class Alchimiste : Personnage
    {
        public Alchimiste(string nom) : base(nom)
        {
            Competences[0] = new JetDePotion();
            Competences.Add(new PotionChangeVie());
            
        }

        public override async Task ExecuterStrategie()
        {
            Personnage cible = ChoisirCible();
            Personnage cibleChangeVie = ChoisirCibleChangeVie();

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

                int indexAleatoireMortVivant = _random.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
                return ciblesMortVivant[indexAleatoireMortVivant];

            }

            int indexAleatoire; // Sélectionner un index aléatoire.
            do
            {
                indexAleatoire = _random.Next(BattleArena.Participants.Count); // Sélectionner un index aléatoire.
            } while (BattleArena.Participants[indexAleatoire] == this || BattleArena.Participants[indexAleatoire].EstMort);

            // Si aucun MortVivant n'est trouvé, tous les autres personnages sont les ennemis.
            return BattleArena.Participants[indexAleatoire];
        }
    }
}
