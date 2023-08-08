using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;

namespace BattleRoyal_RPG.State
{
    internal class EtatEtourdi : EtatDecorateurAsync
    {
        private int DureeEtourdissement; // Durée en millisecondes
        private bool EstActuellementEtourdi = true;
        public EtatEtourdi(IPersonnage Personnage, int dureeEtourdissement = 5) : base(Personnage)
        {
            this.DureeEtourdissement = dureeEtourdissement;
            InitialiserEtourdissement();
        }

        private async void InitialiserEtourdissement()
        {
            await Task.Delay(DureeEtourdissement);
            EstActuellementEtourdi = false;
        }

        public override void Attaquer(Personnage cible)
        {
            if (!EstActuellementEtourdi)
            {
                base.Attaquer(cible);
            }
            // Si le personnage est étourdi, il ne fait rien
        }


    }
}
