using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;

namespace BattleRoyal_RPG.State
{
    public abstract class EtatDecorateurAsync
    {
        protected IPersonnage PersonnageDecore;
        

        public virtual bool Mort
        {
            get { return PersonnageDecore.EstMort; } // Par défaut, un personnage est vivant

        }
        public virtual bool EstAttaquable
        {
            get { return PersonnageDecore.EstAttaquable; } // Par défaut, un personnage est attaquable
        }
        public virtual bool EstMangeable
        {
            get { return PersonnageDecore.EstMangeable; } // Par défaut, un personnage n'est pas mangeable
            set { PersonnageDecore.EstMangeable = value; }
        }

        public EtatDecorateurAsync(IPersonnage personnage)
        {
            PersonnageDecore = personnage;
            
        }

        public virtual int Vie
        {
            get { return PersonnageDecore.Vie; }
            set { PersonnageDecore.Vie = value; }
        }

        public virtual int Attaque
        {
            get { return PersonnageDecore.Attaque; }
            set { PersonnageDecore.Attaque = value; }
        }

        public virtual int Defense
        {
            get { return PersonnageDecore.Defense; }
            set { PersonnageDecore.Defense = value; }
        }

        public virtual void Attaquer(Personnage cible)
        {
            PersonnageDecore.AttaquerBase(cible);
        }



        
    }
}
