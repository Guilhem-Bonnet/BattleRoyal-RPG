using BattleRoyal_RPG.Observeur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Utilities
{
    public class MessageBuilder
    {
        public Message LifeMessage(string nom, int vie, int defense)
        {
            var message = new Message();
            message.AddSegment($"{nom} à {vie}pv et {defense}def \n");
            return message;
        }
    }
}
