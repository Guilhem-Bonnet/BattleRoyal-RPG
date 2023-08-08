using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Classe
{
    internal class Vampire : Personnage
    {
        public Vampire(string nom) : base(nom)
        {
            TypeDuPersonnage = TypePersonnage.MortVivant;
        }

        public override async Task ExecuterStrategie()
        {
            throw new NotImplementedException();
        }
    }
}
