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
        public override string Name => "Mangeur de cadavre";
        public int Gain_life = 0;
        public override float Recharge_Initiale { get; set; } = 2;
   

        public override TypeAttack Type { get; set; } = TypeAttack.Normal;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            if (cible.IsEatable && EstDisponible)
            {
                // Le zombie consomme le cadavre et regagne de la Life
                if (cible.Etats.Any(e => e is EstMange))
                {
                    // Gérer l'erreur - peut-être lancer une exception ou simplement retourner
                    throw new InvalidOperationException("Le cadavre a déjà été consommé.");
                }
                else
                {
                    new EstMange(cible);
                    Gain_life = cible.MaxLife / 2;
                    lanceur.Life += Gain_life;

                    Message message = new Message();
                    message.AddSegment($"{lanceur.Name} utilise  ")
                           .AddSegment($"{Name}", ConsoleColor.Cyan)
                           .AddSegment($" sur {cible.Name}! \n", ConsoleColor.Red)
                           .AddSegment($"il gagne")
                           .AddSegment($" {Gain_life} ", ConsoleColor.Green);

                    Personnage.notify.AddMessageToQueue(message);

                }



            }
            else if (!cible.IsEatable)
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

