using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Decorateur;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Observeur;
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
        public int Gain_vie = 0;
        public override float Recharge_Initiale { get; set; } = 2;
   

        public override TypeAttaque Type { get; set; } = TypeAttaque.Normal;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (cible.EstMangeable && EstDisponible)
            {
                // Le zombie consomme le cadavre et regagne de la vie
                if (cible.Etats.Any(e => e is EstMange))
                {
                    // Gérer l'erreur - peut-être lancer une exception ou simplement retourner
                    throw new InvalidOperationException("Le cadavre a déjà été consommé.");
                }
                else
                {
                    new EstMange(cible);
                    Gain_vie = cible.VieMax / 2;
                    lanceur.Vie += Gain_vie;

                    Message message = new Message();
                    message.AddSegment($"{lanceur.Nom} utilise  ")
                           .AddSegment($"{Nom}", ConsoleColor.Cyan)
                           .AddSegment($" sur {cible.Nom}! \n", ConsoleColor.Red)
                           .AddSegment($"il gagne")
                           .AddSegment($" {Gain_vie} ", ConsoleColor.Green);

                    Personnage.notifier.AddMessageToQueue(message);

                }



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

