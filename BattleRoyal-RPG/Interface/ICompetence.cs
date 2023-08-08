using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Interface
{
    public interface ICompetence
    {
        string Nom { get; }
        float DelaiRecharge { get; set; }
        float Recharge_Initiale { get; set; }
        bool EstDisponible { get; }
        public TypeAttaque Type { get; set; }
        Task Utiliser(Personnage lanceur, Personnage cible);

    }
}
