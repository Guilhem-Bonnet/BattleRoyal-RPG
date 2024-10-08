﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Interface;

namespace BattleRoyal_RPG.Competences
{
    public abstract class Competence : ICompetence
    {
        public virtual string Name => throw new NotImplementedException();

        public virtual float DelaiRecharge { get; set; }
        public virtual float Recharge_Initiale { get; set; } = 1;

        public virtual bool EstDisponible => DelaiRecharge <= 0;

        public virtual TypeAttack Type { get; set; } = TypeAttack.Normal;

        public virtual async Task Utiliser(Personnage lanceur, Personnage cible)
        {

            _ = DiminuerDelaiRecharge();

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
