using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Interface;
/*
Effet : Acide
- Retire de la vie à chaque fois que l'effet est déclenché sur la cible 
- Réduit sa défense pendant 5 secondes (cumulable)

Exemple d'utilisation :

Attaque de l'acide : 2 points de vie retirés et -5 points de défense pendant 5 secondes

---Attente de 1 secondes---

Acide : -5 points de défense pendant 4 secondes

---Attente de 1 secondes---

Acide : -5 points de défense pendant 3 secondes
Attaque de l'acide : 2 points de vie retirés et -10 points de défense pendant 5 secondes
...

*/
namespace BattleRoyal_RPG.State
{
    internal class EtatAcide : Etat
    {
        public override string Nom => "Acide";
        private CancellationTokenSource _cts = new CancellationTokenSource();
        public EtatAcide(Personnage personnage) : base(personnage) { }

        public override async Task Appliquer()
        {
            Console.WriteLine("Attaque de l'acide : 2 points de vie retirés et -5 points de défense pendant 5 secondes");
            _cts.Cancel();  // Annule la précédente instance d'empoisonnement (réinitialise le délai)
            _cts = new CancellationTokenSource();
            // Réduit la vie à cause de l'acide
            Personnage.Vie -= 2;


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
