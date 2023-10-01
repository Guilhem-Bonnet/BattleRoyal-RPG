using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Core
{
    public class BattleArena
    {
        public static List<Personnage> Participants { get; private set; } = new List<Personnage>();
        public static bool EndBattle = true;

        public BattleArena(List<Personnage> participants)
        {
            Participants = participants;
        }

        public async Task StartBattle()
        {
            EndBattle = false;
            Console.WriteLine("Début du combat !");
            var tasks = Participants.Select(participant => Task.Run(() => participant.ExecuterStrategie())).ToArray();


            // Attendre que le combat soit terminé
            while (Participants.Count(p => p.Vie > 0) > 1 &&
                   !(Participants.Count(p => p.Vie > 0 && p.TypeDuPersonnage != TypePersonnage.MortVivant) == 0))
            {

                await Task.Delay(1000);

            }
            EndBattle = true;
            // Affichage des résultats
            foreach (var participant in Participants)
            {
                Console.Write($"{participant.Nom} \n");
                Console.Write($"Vie: {participant.Vie} \n");
                Console.Write($"EstMort: {participant.EstMort}. \n");
                Console.Write($"EstMangeable: {participant.EstMangeable}.\n");
                Console.Write($"EstAttaquable: {participant.EstAttaquable}.\n");
                Console.Write($"\n");
            }
            var survivors = Participants.Where(p => p.Vie > 0).ToList();
            if (survivors.Count == 1)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"{survivors[0].Nom} est le dernier survivant!");
            }
            else if (survivors.All(s => s.TypeDuPersonnage == TypePersonnage.MortVivant))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Les MortVivants ont dominé le champ de bataille!");
            }
            else if (survivors.All(s => s.TypeDuPersonnage == TypePersonnage.Humain))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Les Humains ont dominé le champ de bataille!");
            }
            else
            {
                Console.WriteLine("Tous les participants sont morts. Il n'y a pas de survivants!");
            }

            Console.ResetColor();
        }
    }
}
