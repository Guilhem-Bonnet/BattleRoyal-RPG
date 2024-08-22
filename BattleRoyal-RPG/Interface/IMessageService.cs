using BattleRoyal_RPG.Observeur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Interface
{
    public interface IMessageService
    {
        Message CreateLifeMessage(string name, int life, int defense);
        Message CombineMessages(params Message[] messages);
        void Notify(Message message);
    }
}
