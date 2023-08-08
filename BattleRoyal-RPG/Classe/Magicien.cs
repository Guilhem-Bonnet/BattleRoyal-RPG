using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    internal class Magicien : Personnage
    {
        public Magicien(string nom) : base(nom)
        {
        }

        public override async Task ExecuterStrategie()
        {
            throw new NotImplementedException();
        }
    }
}
