using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG
{
    public abstract class Etat
    {
        public Personnage Personnage { get; }
        protected int Cumul;
        public virtual string Nom { get; } = "";

        public Etat(Personnage personnage)
        {
            Cumul = 1; // Par défaut, le cumul est 1
            Personnage = personnage;
        }

        // Cette méthode sera appelée chaque "tick" ou tour du jeu pour chaque état actif
        public virtual async Task Appliquer() {
        
        }

        // Cette méthode sera appelée lorsque l'état est ajouté
        public virtual async Task Debut() { }

        // Cette méthode sera appelée lorsque l'état est retiré
        public virtual async Task Fin() { }

        public virtual void Cumuler()
        {
            Cumul++;
        }

        public virtual void AnnulerCumul()
        {
            Cumul = 1;
        }
    }


}
