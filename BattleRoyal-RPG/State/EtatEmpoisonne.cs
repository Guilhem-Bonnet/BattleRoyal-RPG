using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;

/*
Effet d'empoisonnement :
- Fait perdre 3 points de vie par seconde pendant 5 secondes
- Cumulable

 */

namespace BattleRoyal_RPG.State
{
    internal class EtatEmpoisonne : Etat
    {
        public override string Nom => "Empoisoné";
        public int degats = 3;
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public EtatEmpoisonne(Personnage personnage) : base(personnage) { 
         
        }


        public override async Task Appliquer()
        {
            _cts.Cancel();  // Annule la précédente instance d'empoisonnement (réinitialise le délai)
            _cts = new CancellationTokenSource();

            for (int i = 0; i < 5; i++)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;
                if (Personnage.EstMort)
                    return;

                var message = new Message(); // Créez une nouvelle instance de message à chaque itération
                message.AddSegment($"{Personnage.Nom} est empoisoné et perd ", ConsoleColor.DarkMagenta)
                       .AddSegment($"{degats}pv\n", ConsoleColor.Red);

                Personnage.Vie -= degats * Cumul;

                if (Personnage.EstMort)
                {
                    message.AddSegment($"{Personnage.Nom} est mort empoisoné", ConsoleColor.DarkMagenta);
                    Personnage.notifier.AddMessageToQueue(message);
                    return;
                }

                Personnage.notifier.AddMessageToQueue(message);
                await Task.Delay(1000); // Attend 1 seconde
            }

            AnnulerCumul(); // Réinitialise le cumul après la fin de l'effet
        }

        public override void Cumuler()
        {
            base.Cumuler();
            _ = Appliquer(); // Redéclenche l'effet pour réinitialiser le délai
        }

    }
}
