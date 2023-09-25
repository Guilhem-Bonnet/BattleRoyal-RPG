using BattleRoyal_RPG.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Observeur
{
    public class ConsoleLogger : IConsoleObserver
    {
        private readonly object _lock = new object();

        public void Update(ConsoleColor color, string message)
        {
            lock (_lock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }

}
