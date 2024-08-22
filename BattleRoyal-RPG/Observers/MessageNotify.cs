using BattleRoyal_RPG.Interface;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Observeur
{
    public class MessageNotify
    {
        private readonly ConcurrentQueue<Message> messageQueue = new ConcurrentQueue<Message>();
        private readonly List<IMessageObserver> observers = new List<IMessageObserver>();

        private static MessageNotify _instance;
        private MessageNotify() { }

        public static MessageNotify Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MessageNotify();
                }
                return _instance;
            }
        }
        public void RegisterObserver(IMessageObserver observer)
        {
            observers.Add(observer);
        }

        public void AddMessageToQueue(Message message)
        {
            messageQueue.Enqueue(message);
            ProcessQueue(); // Ici, nous traitons la file d'attente chaque fois qu'un message est ajouté. 
                            // Selon vos besoins, cela pourrait être déplacé ou déclenché différemment.
        }

        public void ProcessQueue()
        {
            while (messageQueue.TryDequeue(out Message message))
            {
                NotifyObservers(message);
            }
        }

        private void NotifyObservers(Message message)
        {
            foreach (var observer in observers)
            {
                try
                {
                    observer.Update(message);
                }
                catch (Exception ex)
                {
                    // Gérer ou enregistrer l'exception.
                    Console.WriteLine($"Erreur lors de la notification de l'observateur : {ex.Message}");
                }
            }
        }
    }
}
