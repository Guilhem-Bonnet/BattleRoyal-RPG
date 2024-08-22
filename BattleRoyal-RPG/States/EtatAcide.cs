using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;
/*
Effet : Acide
- Retire de la Life à chaque fois que l'effet est déclenché sur la cible 
- Réduit sa défense pendant 5 secondes (cumulable)

Exemple d'utilisation :

Attack de l'acide : 2 points de Life retirés et -5 points de défense pendant 5 secondes

---Attente de 1 secondes---

Acide : -5 points de défense pendant 4 secondes

---Attente de 1 secondes---

Acide : -5 points de défense pendant 3 secondes
Attack de l'acide : 2 points de Life retirés et -10 points de défense pendant 5 secondes
...

*/
namespace BattleRoyal_RPG.State
{
    internal class EtatAcide : Etat
    {
        public override string Name => "Acide";
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public EtatAcide(Personnage personnage) : base(personnage) { }

        public override async Task Appliquer()
        {
            Message message = new Message();
            message.AddSegment($"{Personnage.Name} est attaqué par de l'acide! ", ConsoleColor.DarkGreen);
            Personnage.notify.AddMessageToQueue(message);

            _cts.Cancel();  // Annule la précédente instance d'empoisonnement (réinitialise le délai)
            _cts = new CancellationTokenSource();
            // Réduit la Life à cause de l'acide
            Personnage.Life -= 2;


            for (int i = 0; i < 5; i++)
            {
                if (_cts.Token.IsCancellationRequested)
                    return;

                Personnage.Defense -= 5 * Cumul;
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
