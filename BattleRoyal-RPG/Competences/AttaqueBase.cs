using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;
using BattleRoyal_RPG.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    public class AttackBase : Competence
    {
        private readonly FightService _fightService;

        public AttackBase(FightService fightService)
        {
            _fightService = fightService;
        }
        public override string Name => "Attack de base";
        public override float Recharge_Initiale { get; set; } = 0.5f;

        public override TypeAttack Type { get; set; }

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            var message = new Message();
            // Si la cible n'est pas attaquable, la fonction retourne immédiatement
            if (!cible.IsAttackable)
            {
                message.AddSegment($"La cible {cible.Name} n'est pas attaquable!", ConsoleColor.Red);
                Personnage.notify.AddMessageToQueue(message);
                return;
            }


            message.AddSegment($"{lanceur.Name} utilise  ")
                   .AddSegment($"{Name}", ConsoleColor.Cyan)
                   .AddSegment($" sur {cible.Name}! \n",ConsoleColor.Red);
            Personnage.notify.AddMessageToQueue(message);

            int dommage = lanceur.CalculateDamage(Type, cible);
            _fightService.InfligerDommages(dommage, cible);
            

            await base.Utiliser(lanceur, cible);

        }


        

    }

}
