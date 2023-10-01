using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Observeur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Characters
{
    internal class Zombie : Personnage
    {
        private const int SEUIL_SANTE = 70;  // seuil de santé pour décider s'il doit utiliser "MangeMort" ou non
        private Random _rand = new Random();
        public Zombie(string nom) : base(nom)
        {
            TypeDuPersonnage = TypePersonnage.MortVivant;
            Defense = 0;
            Competences.Add(new MangeMort());
        }

        public override async Task Strategie()
        {
            var competenceMangeMort = Competences.FirstOrDefault(c => c.EstDisponible && c is MangeMort);
            var competenceAttaque = Competences.FirstOrDefault(c => c.EstDisponible && c is AttaqueBase);
            

            if (Vie <= SEUIL_SANTE && competenceMangeMort != null)
            {
                // Cherche une cible morte parmi tous les personnages.
                var cibleMorte = TrouverCibleMorte();
                
                if (cibleMorte != null)
                {
                    Message message = new Message();
                    // Si une cible morte est trouvée, utilise "MangeMort".
                    message.AddSegment($"{Nom} trouve  ")
                           .AddSegment($"{cibleMorte.Nom}", ConsoleColor.Cyan)
                           .AddSegment($" et utilise  ")
                           .AddSegment($"{competenceMangeMort.Nom}", ConsoleColor.Cyan)
                           .AddSegment($" sur {cibleMorte.Nom}! \n", ConsoleColor.Red);
                    notifier.AddMessageToQueue(message);

                    await competenceMangeMort.Utiliser(this, cibleMorte);
                    return;
                }
            }

            // Trouver une cible qui n'est pas un MortVivant.
            var cibleNonMortVivant = TrouverCibleNonMortVivant();

            if (cibleNonMortVivant != null && Competences[0].EstDisponible)
            {
                // Si une cible non MortVivant est trouvée, utilise son attaque de base ou une autre compétence.
                await Competences[0].Utiliser(this, cibleNonMortVivant);
            }
        }

        private Personnage TrouverCibleNonMortVivant()
        {

            List<Personnage> ciblesMortVivant = new List<Personnage>();

            foreach (var participant in BattleArena.Participants)
            {
                if (!participant.EstMort && participant.TypeDuPersonnage != TypePersonnage.MortVivant)
                {
                    ciblesMortVivant.Add(participant);
                }
            }

            if (ciblesMortVivant.Count == 0)
                return null; // Retourner null si aucune cible n'est trouvée.

            int indexAleatoire = _rand.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
            return ciblesMortVivant[indexAleatoire];
        }

        private Personnage TrouverCibleMorte()
        {
            // Supposant que vous avez une méthode pour récupérer tous les personnages.
            return BattleArena.Participants.FirstOrDefault(predicate: p => p.EstMangeable);
        }

    }
}
