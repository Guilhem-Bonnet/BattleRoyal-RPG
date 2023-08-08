using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    internal class Paladin : Personnage
    {
        public Paladin(string nom) : base(nom)
        {
            Competences[0].Type = TypeAttaque.Sacre;
        }

        public override async Task ExecuterStrategie()
        {
            throw new NotImplementedException();
        }
    }
}
