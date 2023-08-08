using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;

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
            {
                _cts.Cancel();  // Annule la précédente instance d'empoisonnement (réinitialise le délai)
                _cts = new CancellationTokenSource();

                for (int i = 0; i < 5; i++)
                {
                    if (_cts.Token.IsCancellationRequested)
                        return;
                    if (Personnage.EstMort)
                        return;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write($"{Personnage.Nom} est empoisoné et perd " );
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($"{degats*Cumul}pv\n");
                    Console.ResetColor();

                    Personnage.Vie -= degats * Cumul;
                    if (Personnage.EstMort)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"{Personnage.Nom} est mort empoisoné");
                        Console.ResetColor();
                        return;
                    }
                    await Task.Delay(1000); // Attend 1 seconde
                }

                AnnulerCumul(); // Réinitialise le cumul après la fin de l'effet

            }
        }
        public override void Cumuler()
        {
            base.Cumuler();
            _ = Appliquer(); // Redéclenche l'effet pour réinitialiser le délai
        }

    }
}
