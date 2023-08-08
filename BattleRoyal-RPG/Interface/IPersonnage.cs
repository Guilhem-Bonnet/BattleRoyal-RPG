using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleRoyal_RPG.Interface
{
    public interface IPersonnage
    {
        string Nom { get; set; }
        int Vie { get; set; }
        int VieMax { get; set; }
        int Attaque { get; set; }
        int Defense { get; set; }
        bool EstAttaquable { get; }   // Pour déterminer si le personnage est attaquable
        bool EstMort { get; }         // Pour déterminer si le personnage est mort
        bool EstMangeable { get; set; }    // Pour déterminer si le personnage est mangeable
        public TypePersonnage TypeDuPersonnage { get; set; }
        void AttaquerBase(Personnage cible);
        Task ExecuterStrategie();
        List<ICompetence> Competences { get; set; }
        ResultatDe LancerDe();


    }
}
