using BattleRoyal_RPG.Competences;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    internal class Zombie : Personnage
    {
        private const int SEUIL_SANTE = 70;  // seuil de santé pour décider s'il doit utiliser "MangeMort" ou non
        private Random _random = new Random();
        public Zombie(string nom) : base(nom)
        {
            TypeDuPersonnage = TypePersonnage.MortVivant;
            Defense = 0;
            Competences.Add(new MangeMort());
        }

        public override async Task ExecuterStrategie()
        {
            Console.WriteLine($"{Nom} à {Vie}pv et {Defense}def");
            var competenceMangeMort = Competences.FirstOrDefault(c => c.EstDisponible && c is MangeMort);
            var competenceAttaque = Competences.FirstOrDefault(c => c.EstDisponible && c is AttaqueBase);


            if (Vie <= SEUIL_SANTE && competenceMangeMort != null && competenceMangeMort.EstDisponible)
            {
                // Cherche une cible morte parmi tous les personnages.
                var cibleMorte = TrouverCibleMorte();

                if (cibleMorte != null)
                {
                    // Si une cible morte est trouvée, utilise "MangeMort".
                    Console.WriteLine($"{Nom} utilise {competenceMangeMort.Nom} sur {cibleMorte.Nom}.");
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
            // Vous devrez adapter cette logique en fonction de la façon dont vous gérez tous les personnages dans votre jeu.

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

            int indexAleatoire = _random.Next(ciblesMortVivant.Count); // Sélectionner un index aléatoire.
            return ciblesMortVivant[indexAleatoire];
        }

        private Personnage TrouverCibleMorte()
        {
            // Supposant que vous avez une méthode pour récupérer tous les personnages.
            return BattleArena.Participants.FirstOrDefault(p => p.EstMangeable);
        }

    }
}
