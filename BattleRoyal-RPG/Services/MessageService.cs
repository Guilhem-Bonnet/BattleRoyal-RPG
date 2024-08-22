using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Services
{
    public class MessageService : IMessageService
    {
        private readonly MessageNotify _notify = MessageNotify.Instance;
        public Message CreateLifeMessage(string name, int life, int defense)
        {
            var message = new Message();
            // Build message
            message.AddSegment($"{name} à {life}pv et {defense}def \n");
            return message;
        }

        public Message CombineMessages(params Message[] messages)
        {
            var mainMessage = new Message();
            // Combine message
            return mainMessage;
        }

        public void Notify(Message message)
        {
            _notify.AddMessageToQueue(message);
        }
    }
}
