using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;
using BattleRoyal_RPG.State;

namespace BattleRoyal_RPG.Decorateur
{
    internal class EstMange : EtatDecorateurAsync
    {
        private readonly object EstMangeableLock = new object();
        public override bool EstMangeable
        {
            get { return false; } // le personnage mort est mangé et n'est plus mangeable.
            set { base.EstMangeable = value; }
        }
        public EstMange(Personnage personnage) : base(personnage)
        {
            lock (EstMangeableLock)
            {
                base.EstMangeable = false; // Mise à jour de la propriété.
                if (!(personnage is EstMort) && EstMangeable)
                {
                    Message message = new Message();
                    message.AddSegment($"{personnage.Nom} est mangé.");
                    message.AddSegment($"{personnage.Nom} n'est plus mangeable.");
                    Personnage.notifier.AddMessageToQueue(message);
                    return;
                }
                else
                {
                    Message message = new Message();
                    message.AddSegment($"quelqu'un essaye de de mangé {personnage.Nom} mais il n'est plus mangeable.");
                    Personnage.notifier.AddMessageToQueue(message);
                    return;
                }
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
