using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Interface
{
    public interface ICompetence
    {
        string Name { get; }
        float DelaiRecharge { get; set; }
        float Recharge_Initiale { get; set; }
        bool EstDisponible { get; }
        public TypeAttack Type { get; set; }
        Task Utiliser(Personnage lanceur, Personnage cible);

    }
}
