using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Characters
{
    internal class Vampire : Personnage
    {
        public Vampire(string nom) : base(nom)
        {
            TypeDuPersonnage = TypePersonnage.MortVivant;
        }

        public override async Task Strategie()
        {
            throw new NotImplementedException();
        }
    }
}
