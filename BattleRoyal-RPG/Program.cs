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
            var zombieD = new Zombie("Zombie Courgette");
            var zombieE = new Zombie("Zombie Myrianne");
            var zombieF = new Zombie("Zombie Gaspacho");
            var pretre = new Pretre("Pretre ATesSouhaits");
            var pretreA = new Pretre("Pretre Bépanten");
            var alchimiste = new Alchimiste("Alchimiste Archimed");
            var alchimisteA = new Alchimiste("Alchimiste Paul");

            var arena = new BattleArena(new List<Personnage> { alchimiste,alchimisteA, zombie, zombieA, zombieB, zombieC, zombieD, zombieE, zombieF, pretre, pretreA });

            Console.WriteLine("Le combat commence !");

            // Démarrer le combat
            await arena.StartBattle();

            Console.WriteLine("Le combat est terminé !");

            Console.WriteLine("Appuyez sur Entrée pour quitter...");
            Console.ReadLine();  // Cela empêchera la console de se fermer immédiatement.

        }
    }
}