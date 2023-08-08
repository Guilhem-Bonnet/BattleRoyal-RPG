using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competence
{
    public class Competence : ICompetence
    {
        public virtual string Nom => throw new NotImplementedException();

        public virtual float DelaiRecharge { get ; set ; }
        public virtual float Recharge_Initiale { get ; set; } = 1;

        public virtual bool EstDisponible => DelaiRecharge <= 0;

        public virtual TypeAttaque Type { get ; set ; } = TypeAttaque.Normal;

        public virtual async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            
            await DiminuerDelaiRecharge();

        }

        private async Task DiminuerDelaiRecharge()
        {
            DelaiRecharge = Recharge_Initiale;

            while (DelaiRecharge > 0)
            {
                // Prenons la plus petite valeur entre DelaiRecharge et 1f pour le delay
                float delay = Math.Min(DelaiRecharge, 1f);
                await Task.Delay(TimeSpan.FromSeconds(delay));
                DelaiRecharge -= delay;  // Réduit le délai par le montant fractionné
            }
        }
    }
}
