using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.State;

namespace BattleRoyal_RPG.Decorateur
{
    internal class EstMort : EtatDecorateurAsync
    {

        public EstMort(IPersonnage personnage) : base(personnage)
        {

        }
        public override bool EstAttaquable
        {
            get { return false; } // Un personnage mort n'est pas attaquable
        }
        public override bool EstMangeable
        {
            get { return true; } // Un personnage mort est mangeable
        }
        public override bool Mort
        {
            get { return true; } // Un personnage dans l'état "Mort" est effectivement mort
        }

        public override int Vie
        {
            get { return 0; } // Un personnage mort n'a plus de vie.
            set { } // Ne rien faire, car la vie ne peut pas être modifiée une fois mort.

        }

        public override void Attaquer(Personnage cible)
        {
            // Un personnage mort ne peut pas attaquer.
            // Ne rien faire ou peut-être afficher un message d'erreur.
        }

    }
}