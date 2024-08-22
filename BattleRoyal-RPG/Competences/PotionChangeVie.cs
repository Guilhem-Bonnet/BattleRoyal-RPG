using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Observeur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    public class PotionChangeLife : Competence
    {
        public override string Name => "Potion SwitchLife";
        public const int Gain_life=0;
        public override float Recharge_Initiale { get; set; } = 3.2f;
        public override TypeAttack Type { get; set; } = TypeAttack.Sacre;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            Message message = new Message();
            if (!cible.IsAttackable)
            {
                message.AddSegment($"La cible {cible.Name} n'est pas attaquable!", ConsoleColor.Red);
                Personnage.notify.AddMessageToQueue(message);
                return;
            }
            message.AddSegment($"{lanceur.Name} utilise  ")
                   .AddSegment($"{Name}", ConsoleColor.Cyan)
                   .AddSegment($" sur {cible.Name}! \n", ConsoleColor.Red);
            


            int LifeActuelle = lanceur.Life;
            int LifeCible = cible.Life;   

            if (LifeCible>lanceur.MaxLife)
            {
                LifeCible = lanceur.MaxLife;
            }

            lanceur.Life = LifeCible;
            cible.Life = LifeActuelle;

            message.AddSegment($"{lanceur.Name} échange sa Life avec {cible.Name} ")
                   .AddSegment($"{cible.Name} à maintenant {cible.Life} et {lanceur.Name} à {lanceur.Life} \n");
            Personnage.notify.AddMessageToQueue(message);



            base.Utiliser(lanceur, cible);
        }
    }

}
