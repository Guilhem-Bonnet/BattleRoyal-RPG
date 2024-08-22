using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;

namespace BattleRoyal_RPG.Characters
{
    internal class Necromancien : Personnage
    {
        public Necromancien(string Name) : base(Name)
        {
        }

        public override async Task Strategie()
        {
            throw new NotImplementedException();
        }
    }
}
