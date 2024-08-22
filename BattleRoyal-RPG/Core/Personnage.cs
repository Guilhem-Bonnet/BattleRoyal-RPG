using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Competences;
using BattleRoyal_RPG.Observeur;
using System.Diagnostics.Metrics;
using System.Diagnostics;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Characters;
using BattleRoyal_RPG.Services;

namespace BattleRoyal_RPG.Core
{
    public abstract class Personnage : IPersonnage
    {
        #region Champs
        private int _life;
        private bool? _isEatable;
        private readonly object stateLock = new object();
        protected Random _rand = new Random();
        const int FACE_DE = 20;
        public static readonly MessageNotify notify = MessageNotify.Instance;
        #endregion

        #region Propriétés
        public string Name { get; set; }
        public int MaxLife { get; set; } = 100;
        public int Attack { get; set; } = 5;
        public int Defense { get; set; } = 10;
        public TypePersonnage TypeDuPersonnage { get; set; } = TypePersonnage.Humain;
        public Personnage etatOriginal;
        
        protected readonly IFightService _fightservice;

        public virtual bool IsAttackable => !IsDead;
        public virtual bool IsDead => Life <= 0;
        public virtual bool IsEatable
        {
            get => _isEatable ?? IsDead;
            set => _isEatable = value;
        }

        public int Life
        {
            get { return _life; }
            set
            {
                _life = value;
            }
        }
        #endregion

        #region Collections
        public List<Etat> Etats { get; } = new List<Etat>();
        public List<ICompetence> Competences { get; set; } = new List<ICompetence>();
        #endregion

        #region Constructeurs
        public Personnage(string name)
        {
            etatOriginal = this;
            Name = name;
            _life = MaxLife;
            var messageService = new MessageService(); // Supposons que tu as déjà une implémentation de IMessageService
            _fightservice = new FightService(messageService);
            Competences.Add(new AttackBase((FightService)_fightservice));


        }
        #endregion

        #region Méthodes

        #region Méthodes de combat

        public void AttackBase(Personnage cible)
        {
            Competences[0].Utiliser(this, cible);
        }
        public virtual int CalculateDamage(TypeAttack typeAttack, Personnage cible, int attack = -1)
        {
            ResultDe resultAttack = LancerDe();
            ResultDe resultDefense = cible.LancerDe();

            if (attack < 0) attack = Attack;
            return _fightservice.CalculateDamage(this, cible, typeAttack, resultAttack, resultDefense);
        }

        #endregion

        #region Méthodes de Gestion des États
        public ResultDe LancerDe()
        {
            int result = _rand.Next(1, FACE_DE + 1);

            if (result == 1)
                return ResultDe.EchecCritique;
            else if (result == 20)
                return ResultDe.RéussiteCritique;
            else if (result <= 5)
                return ResultDe.Echec;
            else if (result >= 16)
                return ResultDe.Réussite;
            else
                return ResultDe.Neutre;
        }
        public void AjouterEtat(Etat etat)
        {
            Etats.Add(etat);
            _ = etat.Appliquer();
        }
        public void AppliquerOuCumulerEtat(Etat etat)
        {
            lock (stateLock) // Lock sur un objet dédié pour éviter les deadlocks
            {
                var etatType = etat.GetType();
                var etatExistant = Etats.FirstOrDefault(e => e.GetType() == etatType);

                if (etatExistant != null)
                {
                    etatExistant.Cumuler(); // Si un état du même type existe déjà, cumuler cet état.
                }
                else
                {
                    AjouterEtat(etat); // Sinon, ajouter le nouvel état à la liste des états du personnage.
                }
            }
        }
        public void RetirerEtat<T>() where T : Etat
        {
            var etat = Etats.OfType<T>().FirstOrDefault();
            if (etat != null)
            {
                etat.Fin();
            }
        }
        public bool AEtat<T>() where T : Etat
        {
            return Etats.OfType<T>().Any();
        }
        #endregion

        #region Méthodes de Stratégie
        public async Task ExecuterStrategie()
        {

            while (Life > 0 && !BattleArena.EndBattle)
            {
                await Strategie();
            }

        }
        public virtual async Task Strategie()
        {

        }
        #endregion



        #endregion

    }
}
