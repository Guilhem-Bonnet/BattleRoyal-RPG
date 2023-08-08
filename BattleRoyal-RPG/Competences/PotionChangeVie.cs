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
            if (!cible.EstAttaquable)
            {
                Console.WriteLine("La cible n'est pas attaquable!");
                return;
            }
            Console.Write($"{lanceur.Nom} utilise  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($" {Nom} ");
            Console.ResetColor();
            Console.WriteLine($"sur {cible.Nom}! \n");

            int VieActuelle = lanceur.Vie;
            int VieCible = cible.Vie;   

            if (VieCible>lanceur.VieMax)
            {
                VieCible = lanceur.VieMax;
            }

            lanceur.Vie = VieCible;
            cible.Vie = VieActuelle;

            Console.Write($"{lanceur.Nom} échange sa vie avec {cible.Nom} ");
            Console.WriteLine($"{cible.Nom} à maintenant {cible.Vie} et {lanceur.Nom} à {lanceur.Vie} \n");

            base.Utiliser(lanceur, cible);
        }
    }

}
