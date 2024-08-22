using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Characters
{
    internal class Paladin : Personnage
    {
        public Paladin(string Name) : base(Name)
        {
            Competences[0].Type = TypeAttack.Sacre;
        }

        public override async Task Strategie()
        {
            throw new NotImplementedException();
        }
    }
}
