using BattleRoyal_RPG.Classe;
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
        public override float Recharge_Initiale { get; set; } = 3;
        public override TypeAttaque Type { get; set; } = TypeAttaque.Sacre;
        public int valueSoin { get; set; } = 20;
        public int valueDommage { get; set; } = 20;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (EstDisponible)
            {

                Console.Write($"{lanceur.Nom} utilise  ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($" {Nom} ");
                Console.ResetColor();
                Console.WriteLine($"sur {cible.Nom}! \n");

                // Soins ou dommages basés sur le type de cible
                if (cible.TypeDuPersonnage == TypePersonnage.MortVivant)
                {
                    int degats = lanceur.CalculerDommage(lanceur.LancerDe(), cible.LancerDe(), Type, cible, valueDommage);// inflige des dégâts aux MortVivant
                    
                    cible.Vie -= degats;

                    Console.Write($"{lanceur.Nom} inflige ");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write($" {degats} ");
                    Console.ResetColor();
                    Console.WriteLine($"{cible.Nom} à {cible.Nom} avec {Nom} \n");

                }
                else
                {
                    cible.Vie += valueSoin; // guérit les autres

                    Console.Write($"{lanceur.Nom} soigne {cible.Nom} avec {Nom} et regagne");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write($" {valueSoin} ");
                    Console.ResetColor();
                    Console.WriteLine("points de vie! \n");

                }
                Console.ResetColor();

                base.Utiliser(lanceur, cible);
            }
        }
    }
}
