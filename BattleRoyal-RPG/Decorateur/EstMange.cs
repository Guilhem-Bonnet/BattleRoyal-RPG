using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.State;

namespace BattleRoyal_RPG.Decorateur
{
    internal class EstMange : EtatDecorateurAsync
    {
        public override bool EstMangeable
        {
            get { return false; } // le personnage mort est mangé et n'est plus mangeable.
            set { base.EstMangeable = value; }
        }
        public EstMange(IPersonnage personnage) : base(personnage)
        {
            base.EstMangeable = false; // Mise à jour de la propriété.
            if (!(personnage is EstMort))
            {
                Console.WriteLine($"{personnage.Nom} n'est plus mort.");
                return; 
            }
        }

        public override int Vie
        {
            get { return 0; } // Un personnage mangé n'a plus de vie.
            set { } // Ne rien faire, car la vie ne peut pas être modifiée une fois mangée.
        }

        public override void Attaquer(Personnage cible)
        {
            // Un personnage mangé ne peut pas attaquer. 
            // Ne rien faire ou peut-être afficher un message d'erreur.
        }

    }
}
