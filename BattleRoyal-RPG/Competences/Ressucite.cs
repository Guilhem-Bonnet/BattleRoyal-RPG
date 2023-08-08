using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Competences
{
    public class Ressucite : Competence
    {
        public override string Nom => "Mangeur de cadavre";
        public override float Recharge_Initiale { get; set; } = 10;
        public override TypeAttaque Type { get; set; } = TypeAttaque.Sacre;

        public override async Task Utiliser(Personnage lanceur, Personnage cible)
        {
            // Supprimez tous les états/decorateurs et rétablissez le personnage à son état original
            cible = cible.etatOriginal;
            // Puis, vous pouvez ajouter des points de vie, ou faire d'autres configurations nécessaires à la résurrection.
            cible.Vie = 100; // Par exemple, restaurer à 100 points de vie.

            base.Utiliser(lanceur, cible);
        }

    }
}
