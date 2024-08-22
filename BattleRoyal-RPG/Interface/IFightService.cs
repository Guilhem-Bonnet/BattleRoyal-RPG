using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Interface
{
    public interface IFightService
    {
        int CalculateDamage(Personnage attacker, Personnage defender, TypeAttack typeAttack, ResultDe resultAttack, ResultDe resultDefense);
    }
}
