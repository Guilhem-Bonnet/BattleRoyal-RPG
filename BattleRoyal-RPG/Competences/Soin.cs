using BattleRoyal_RPG.Classe;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Observeur;
using BattleRoyal_RPG.Services;
using BattleRoyal_RPG.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    public class Soin : Competence
    {
        public override string Name => "Soin";
        public override float Recharge_Initiale { get; set; } = 2.5f;
        public override TypeAttack Type { get; set; } = TypeAttack.Sacre;
        public int valueSoin { get; set; } = 20;
        public int valueDommage { get; set; } = 20;

        private readonly FightService _fightService;

        public Soin(FightService fightService)
        {
            _fightService = fightService;
        }

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (EstDisponible)
            {
                Message message = new Message();
                message.AddSegment($"{lanceur.Name} utilise  ")
                       .AddSegment($"{Name}", ConsoleColor.Cyan)
                       .AddSegment($" sur {cible.Name}! \n", ConsoleColor.Red);


                // Soins ou dommages basés sur le type de cible
                if (cible.TypeDuPersonnage == TypePersonnage.MortVivant)
                {
                    int degats = lanceur.CalculateDamage(Type, cible, valueDommage);// inflige des dégâts aux MortVivant

                    _fightService.InfligerDommages(degats, cible);

                    message.AddSegment($"{lanceur.Name} inflige ")
                           .AddSegment($"{degats} ", ConsoleColor.Red)
                           .AddSegment($" à {cible.Name} avec {Name} \n");

                }
                else
                {
                    if (cible.Etats.Exists(e => e is EtatEmpoisonne))
                    {
                        cible.RetirerEtat<EtatEmpoisonne>();
                        message.AddSegment($"{cible.Name} n'est plus empoisonné! \n", ConsoleColor.Green);
                    }
                    cible.Life += valueSoin; // guérit les autres

                    message.AddSegment($"{lanceur.Name} soigne ")
                           .AddSegment($"{cible.Name} ", ConsoleColor.Green)
                           .AddSegment($" avec {Name} et regagne ")
                           .AddSegment($"{valueSoin} ", ConsoleColor.Green)
                           .AddSegment($" points de Life! \n");

                }
                Personnage.notify.AddMessageToQueue(message);

                base.Utiliser(lanceur, cible);
            }
        }
    }
}
