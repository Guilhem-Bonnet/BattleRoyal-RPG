using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Interface
{
    public interface IPersonnage
    {
        string Name { get; set; }
        int Life { get; set; }
        int MaxLife { get; set; }
        int Attack { get; set; }
        int Defense { get; set; }
        bool IsAttackable { get; }   // Pour déterminer si le personnage est attaquable
        bool IsDead { get; }         // Pour déterminer si le personnage est mort
        bool IsEatable { get; set; }    // Pour déterminer si le personnage est mangeable
        TypePersonnage TypeDuPersonnage { get; set; }
        void AttackBase(Personnage cible);
        Task Strategie();
        List<ICompetence> Competences { get; set; }
        ResultDe LancerDe();



    }
}
