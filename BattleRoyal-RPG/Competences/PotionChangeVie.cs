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
    public class PotionChangeVie : Competence
    {
        public override string Nom => "Potion SwitchLife";
        public const int Gain_vie=0;
        public override float Recharge_Initiale { get; set; } = 3.2f;
        public override TypeAttaque Type { get; set; } = TypeAttaque.Sacre;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            Message message = new Message();
            if (!cible.EstAttaquable)
            {
                message.AddSegment($"La cible {cible.Nom} n'est pas attaquable!", ConsoleColor.Red);
                Personnage.notifier.AddMessageToQueue(message);
                return;
            }
            message.AddSegment($"{lanceur.Nom} utilise  ")
                   .AddSegment($"{Nom}", ConsoleColor.Cyan)
                   .AddSegment($" sur {cible.Nom}! \n", ConsoleColor.Red);
            


            int VieActuelle = lanceur.Vie;
            int VieCible = cible.Vie;   

            if (VieCible>lanceur.VieMax)
            {
                VieCible = lanceur.VieMax;
            }

            lanceur.Vie = VieCible;
            cible.Vie = VieActuelle;

            message.AddSegment($"{lanceur.Nom} échange sa vie avec {cible.Nom} ")
                   .AddSegment($"{cible.Nom} à maintenant {cible.Vie} et {lanceur.Nom} à {lanceur.Vie} \n");
            Personnage.notifier.AddMessageToQueue(message);



            base.Utiliser(lanceur, cible);
        }
    }

}
