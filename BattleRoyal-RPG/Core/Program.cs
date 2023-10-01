using BattleRoyal_RPG.Classe;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using BattleRoyal_RPG.Observeur;
using BattleRoyal_RPG.helper;
using BattleRoyal_RPG.Characters;

namespace BattleRoyal_RPG.Core
{
    public class Program
    {

        static async Task Main(string[] args)
        {
            // Rediriger la sortie standard vers un fichier texte


            var notifier = MessageNotifier.Instance;
            var consoleObserver = new ConsoleObserver();
            notifier.RegisterObserver(consoleObserver);

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

            var arena = new BattleArena(new List<Personnage> { pretreA, alchimiste, alchimisteA, zombie, zombieA, zombieB, zombieC, zombieD, zombieE, zombieF, pretre });

            Console.WriteLine("Le combat commence !");

            // Démarrer le combat
            await arena.StartBattle();

            Console.WriteLine("Le combat est terminé !");

            Console.WriteLine("Appuyez sur Entrée pour quitter...");
            Console.ReadLine();  // Cela empêchera la console de se fermer immédiatement.

            // Flush les logs à la fin
            await Logger.Instance.FlushLogToFileAsync("consoleOutput.txt");


        }
    }
}