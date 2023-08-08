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

            // Si la cible n'est pas attaquable, la fonction retourne immédiatement
            if (!cible.EstAttaquable)
            {
                Console.WriteLine("La cible n'est pas attaquable!");
                return;
            }

            ResultatDe resultatAttaque = lanceur.LancerDe();
            ResultatDe resultatDefense = cible.LancerDe();


            Console.Write($"{lanceur.Nom} utilise  ");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($" {Nom} ");
            Console.ResetColor();
            Console.WriteLine($"sur {cible.Nom}! \n");

            int dommage = lanceur.CalculerDommage(resultatAttaque, resultatDefense, Type, cible);
            cible.Vie -= dommage;
            

            Console.Write($"{cible.Nom} subit  ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($" {dommage} ");
            Console.ResetColor();
            Console.WriteLine("points de dégâts! \n");

            base.Utiliser(lanceur, cible);

        }


        

    }

}
