using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;

namespace BattleRoyal_RPG
{
    public abstract class Etat
    {
        public Personnage Personnage { get; }
        protected int Cumul=0;
        protected CancellationTokenSource _cts = new CancellationTokenSource();
        public virtual string Name { get; } = "";

        public Etat(Personnage personnage)
        {
            Personnage = personnage;
        }

        // Cette méthode sera appelée chaque "tick" ou tour du jeu pour chaque état actif
        public virtual async Task Appliquer() {
           await Debut();
        }

        // Cette méthode sera appelée lorsque l'état est ajouté
        public virtual async Task Debut() { }

        // Cette méthode sera appelée lorsque l'état est retiré
        public virtual void Fin() {
            _cts.Cancel();
            AnnulerCumul();
            Personnage.Etats.Remove(this);
        }

        public virtual void Cumuler()
        {
            Cumul+=1;
        }

        public virtual void AnnulerCumul()
        {
            Cumul = 0;
        }
    }


}
