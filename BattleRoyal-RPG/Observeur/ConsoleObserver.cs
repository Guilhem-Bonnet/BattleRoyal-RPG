using BattleRoyal_RPG.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Observeur
{
    public class ConsoleObserver : IMessageObserver
    {
        public void Update(Message message)
        {
            foreach (var segment in message.Segments)
            {
                Console.ForegroundColor = segment.Color;
                Console.Write(segment.Content);
            }
            Console.WriteLine();  // Pour retourner à la ligne après le message
        }
    }

}
