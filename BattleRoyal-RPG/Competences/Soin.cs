using BattleRoyal_RPG.Classe;
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
    public class Soin : Competence
    {
        public override string Nom => "Soin";
        public override float Recharge_Initiale { get; set; } = 2.5f;
        public override TypeAttaque Type { get; set; } = TypeAttaque.Sacre;
        public int valueSoin { get; set; } = 20;
        public int valueDommage { get; set; } = 20;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (EstDisponible)
            {
                Message message = new Message();
                message.AddSegment($"{lanceur.Nom} utilise  ")
                       .AddSegment($"{Nom}", ConsoleColor.Cyan)
                       .AddSegment($" sur {cible.Nom}! \n", ConsoleColor.Red);


                // Soins ou dommages basés sur le type de cible
                if (cible.TypeDuPersonnage == TypePersonnage.MortVivant)
                {
                    int degats = lanceur.CalculerDommage(lanceur.LancerDe(), cible.LancerDe(), Type, cible, valueDommage);// inflige des dégâts aux MortVivant
                    
                    lanceur.InfligerDommages(degats, cible);

                    message.AddSegment($"{lanceur.Nom} inflige ")
                           .AddSegment($"{degats} ", ConsoleColor.Red)
                           .AddSegment($" à {cible.Nom} avec {Nom} \n");

                }
                else
                {
                    cible.Vie += valueSoin; // guérit les autres

                    message.AddSegment($"{lanceur.Nom} soigne ")
                           .AddSegment($"{cible.Nom} ", ConsoleColor.Green)
                           .AddSegment($" avec {Nom} et regagne ")
                           .AddSegment($"{valueSoin} ", ConsoleColor.Green)
                           .AddSegment($" points de vie! \n");

                }
                Personnage.notifier.AddMessageToQueue(message);

                base.Utiliser(lanceur, cible);
            }
        }
    }
}
