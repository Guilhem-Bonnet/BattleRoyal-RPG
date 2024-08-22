using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;

/*
Effet d'empoisonnement :
- Fait perdre 3 points de Life par seconde pendant 5 secondes
- Cumulable

 */

namespace BattleRoyal_RPG.State
{
    internal class EtatEmpoisonne : Etat
    {
        public override string Name => "Empoisoné";
        public int degats = 3;
        
        private int duree = 2;

        public EtatEmpoisonne(Personnage personnage) : base(personnage) {
        }


        public override async Task Appliquer()
        {
            Cumuler();
            _cts.Cancel();
            _cts = new CancellationTokenSource();

            await base.Appliquer();
            var message = new Message(); // Créez une nouvelle instance de message à chaque itération
            
            

            for (int i = 0; i < duree*Cumul; i++)
            {

                if (_cts.Token.IsCancellationRequested)
                    return;
                if (Personnage.IsDead)
                {
                    Fin();
                    return;
                }

                message.Clear(); // Clear l'ancien message
                message.AddSegment($"{Personnage.Name} est empoisoné et perd ", ConsoleColor.DarkMagenta)
                       .AddSegment($"{degats * Cumul}pv ", ConsoleColor.Red) // Montrer les dégâts augmentés
                       .AddSegment($"(Stacks de poison: {Cumul})", ConsoleColor.Yellow);


                Personnage.Life -= degats * Cumul;

                if (Personnage.IsDead)
                {
                    message.AddSegment($"{Personnage.Name} est mort empoisoné", ConsoleColor.DarkMagenta);
                    Personnage.notify.AddMessageToQueue(message);
                    Fin();
                    return;
                }

                Personnage.notify.AddMessageToQueue(message);
                await Task.Delay(1000); // Attend 1 seconde
            }

            AnnulerCumul(); // Réinitialise le cumul après la fin de l'effet
        }

        public override void Cumuler()
        {
            base.Cumuler();
        }

    }
}
