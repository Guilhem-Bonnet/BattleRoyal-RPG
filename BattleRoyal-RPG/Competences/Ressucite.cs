using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;

namespace BattleRoyal_RPG.Competences
{
    public class Ressucite : Competence
    {
        public override string Name => "Mangeur de cadavre";
        public override float Recharge_Initiale { get; set; } = 10;
        public override TypeAttack Type { get; set; } = TypeAttack.Sacre;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            // Supprimez tous les états/decorateurs et rétablissez le personnage à son état original
            cible = cible.etatOriginal;
            // Puis, vous pouvez ajouter des points de Life, ou faire d'autres configurations nécessaires à la résurrection.
            cible.Life = 100; // Par exemple, restaurer à 100 points de Life.

            base.Utiliser(lanceur, cible);
        }

    }
}
