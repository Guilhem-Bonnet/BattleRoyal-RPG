using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG
{
    public class BattleArena
    {
        public static List<Personnage> Participants { get; private set; } = new List<Personnage>();

        public BattleArena(List<Personnage> participants)
        {
            Participants = participants;
        }

        public async Task StartBattle()
        {
            while (Participants.Count(p => p.Vie > 0) > 1 &&
                   !(Participants.Count(p => p.Vie > 0 && p.TypeDuPersonnage != TypePersonnage.MortVivant) == 0))
            {
                foreach (var participant in Participants.Where(p => p.Vie > 0))
                {
                    participant.ExecuterStrategie();

                    // Pour que chaque action ait un peu de délai et que le combat ne se termine pas instantanément
                    await Task.Delay(500);
                }
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
            else
            {
                Console.WriteLine("Tous les participants sont morts. Il n'y a pas de survivants!");
            }

            Console.ResetColor();
        }
    }
}
