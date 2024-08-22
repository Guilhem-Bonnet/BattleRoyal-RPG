using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Interface;

namespace BattleRoyal_RPG.State
{
    public abstract class EtatDecorateurAsync
    {
        protected IPersonnage PersonnageDecore;
        

        public virtual bool Mort
        {
            get { return PersonnageDecore.IsDead; } // Par défaut, un personnage est vivant

        }
        public virtual bool IsAttackable
        {
            get { return PersonnageDecore.IsAttackable; } // Par défaut, un personnage est attaquable
        }
        public virtual bool IsEatable
        {
            get { return PersonnageDecore.IsEatable; } // Par défaut, un personnage n'est pas mangeable
            set { PersonnageDecore.IsEatable = value; }
        }

        public EtatDecorateurAsync(IPersonnage personnage)
        {
            PersonnageDecore = personnage;
            
        }

        public virtual int Life
        {
            get { return PersonnageDecore.Life; }
            set { PersonnageDecore.Life = value; }
        }

        public virtual int AttackValue
        {
            get { return PersonnageDecore.Attack; }
            set { PersonnageDecore.Attack = value; }
        }

        public virtual int DefenseValue
        {
            get { return PersonnageDecore.Defense; }
            set { PersonnageDecore.Defense = value; }
        }

        public virtual void Attack(Personnage cible)
        {
            PersonnageDecore.AttackBase(cible);
        }



        
    }
}
