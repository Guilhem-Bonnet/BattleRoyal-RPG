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
    public class AttaqueBase : Competence
    {
        public override string Nom => "Attaque de base";
        public override float Recharge_Initiale { get; set; } = 0.5f;

        public override TypeAttaque Type { get; set; }

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            var message = new Message();
            // Si la cible n'est pas attaquable, la fonction retourne immédiatement
            if (!cible.EstAttaquable)
            {
                message.AddSegment($"La cible {cible.Nom} n'est pas attaquable!", ConsoleColor.Red);
                Personnage.notifier.AddMessageToQueue(message);
                return;
            }

            ResultatDe resultatAttaque = lanceur.LancerDe();
            ResultatDe resultatDefense = cible.LancerDe();

            message.AddSegment($"{lanceur.Nom} utilise  ")
                   .AddSegment($"{Nom}", ConsoleColor.Cyan)
                   .AddSegment($" sur {cible.Nom}! \n",ConsoleColor.Red);
            Personnage.notifier.AddMessageToQueue(message);

            int dommage = lanceur.CalculerDommage(resultatAttaque, resultatDefense, Type, cible);
            lanceur.InfligerDommages(dommage, cible);
            

            await base.Utiliser(lanceur, cible);

        }


        

    }

}
