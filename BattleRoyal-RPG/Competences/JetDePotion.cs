using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    internal class JetDePotion : AttaqueBase
    {
        public override string Nom => "Jet de fiole";

        public override Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (!cible.EstMort || cible != null)
            {
                cible.AppliquerOuCumulerEtat(new EtatEmpoisonne(cible));
                return base.Utiliser(lanceur, cible);
            }
            return Task.CompletedTask;
            

        }
    }
   
}
