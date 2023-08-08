using BattleRoyal_RPG.Decorateur;
using BattleRoyal_RPG.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    public class MangeMort : Competence
    {
        public override string Nom => "Mangeur de cadavre";
        public const int Gain_vie = 20;
        public override float Recharge_Initiale { get; set; } = 2;
   

        public override TypeAttaque Type { get; set; } = TypeAttaque.Normal;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (cible.EstMangeable && EstDisponible)
            {
                // Le zombie consomme le cadavre et regagne de la vie

                lanceur.Vie += Gain_vie;
                new EstMange(cible);

                Console.Write($"{lanceur.Nom} ");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"utilise {Nom} ");
                Console.ResetColor();
                Console.Write($"et consomme le cadavre de {cible.Nom} et regagne");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($" {Gain_vie} ");
                Console.ResetColor();
                Console.WriteLine("points de vie! \n");

                

            }
            else if (!cible.EstMangeable)
            {
                // Gérer l'erreur - peut-être lancer une exception ou simplement retourner
                throw new InvalidOperationException("La cible doit être un cadavre pour utiliser cette compétence.");
            }
            else if (!EstDisponible)
            {
                // Gérer l'erreur - la compétence n'est pas encore disponible
                throw new InvalidOperationException("La compétence Mangeur de cadavre n'est pas encore disponible.");
            }

            base.Utiliser(lanceur, cible);
            
        }

    }
}

