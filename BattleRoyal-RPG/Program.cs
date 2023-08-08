using BattleRoyal_RPG.Classe;
using BattleRoyal_RPG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace BattleRoyal_RPG
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            // Initialisation des participants
            var zombie = new Zombie("Zombie Gaetan");
            var zombieA = new Zombie("Zombie Rémond");
            var zombieB = new Zombie("Zombie Bertrand");
            var zombieC = new Zombie("Zombie Coline");
            var pretre = new Pretre("Pretre ATesSouhaits");
            var pretreA = new Pretre("Pretre Bépanten");

            var arena = new BattleArena(new List<Personnage> { zombie, zombieA, zombieB, pretre});

            Console.WriteLine("Le combat commence !");

            // Démarrer le combat
            await arena.StartBattle();

            Console.WriteLine("Le combat est terminé !");

            Console.WriteLine("Appuyez sur Entrée pour quitter...");
            Console.ReadLine();  // Cela empêchera la console de se fermer immédiatement.

        }
    }
}