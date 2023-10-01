using BattleRoyal_RPG.helper;
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
        private readonly object _consoleLock = new object();

        public void Update(Message message)
        {
            lock (_consoleLock) // lock sur l'objet pour s'assurer que seul un thread peut accéder à ce bloc à la fois
            {
                
                // Log le message avec le Logger
                Logger.Instance.Log(message);


                foreach (var segment in message.Segments)
                {
                    Console.ForegroundColor = segment.Color;
                    Console.Write(segment.Content);
                }
                Console.ResetColor(); // Reset la couleur de la console
                Console.WriteLine(); // Espace entre les messages
                Console.WriteLine(); // Pour retourner à la ligne après le message
            }
        }
    }


}
