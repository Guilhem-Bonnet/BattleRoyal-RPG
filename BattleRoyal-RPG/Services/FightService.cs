using BattleRoyal_RPG.Core;
using BattleRoyal_RPG.Enums;
using BattleRoyal_RPG.Interface;
using BattleRoyal_RPG.Observeur;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BattleRoyal_RPG.Services
{
    public class FightService : IFightService
    {
        private const int EchecCritiqueMultiplier = 0;
        private const int NeutreMultiplier = 1;
        private const int ReussiteMultiplier = 2;

        private readonly IMessageService _messageService;
        public FightService(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public int CalculateDamage(Personnage attacker, Personnage defender, TypeAttack typeAttack, ResultDe resultAttack, ResultDe resultDefense)
        {
            // Implémentation
            var mainMessage = _messageService.CreateLifeMessage(attacker.Name, attacker.Life, attacker.Defense);
            
            var (baseDommage, casSpecifiquesMessage) = TraiterCasSpecifiquesEtConstruireMessage(resultAttack, resultDefense, typeAttack, defender, attacker, attacker.Attack);
            mainMessage.AddSegmentsFrom(casSpecifiquesMessage);

            var (adjustedDommage, vulnerabilitesMessage) = CalculerVulnerabilitesEtConstruireMessage(baseDommage, typeAttack, attacker, defender);
            mainMessage.AddSegmentsFrom(vulnerabilitesMessage);

            _messageService.Notify(mainMessage);

            return adjustedDommage;
        }
        private (int, Message) TraiterCasSpecifiquesEtConstruireMessage(ResultDe resultAttack, ResultDe resultDefense, TypeAttack typeAttack, IPersonnage cible, IPersonnage attacker, int Attack)
        {
            var message = new Message();
            int dommageAttack = ResultAttack(resultAttack, Attack);
            int totalDefense = ResultDefense(resultDefense, cible);
            int baseDommage = dommageAttack - totalDefense;

            if (resultAttack == ResultDe.RéussiteCritique && resultDefense == ResultDe.RéussiteCritique)
            {
                message.AddSegment($"Choc épique ! ", ConsoleColor.Magenta)
                    .AddSegment($"{attacker.Name}", ConsoleColor.Red)
                    .AddSegment($" et ", ConsoleColor.Magenta)
                    .AddSegment($"{cible.Name}", ConsoleColor.Red)
                    .AddSegment($" démontrent une maîtrise incroyable !", ConsoleColor.Magenta);
            }
            else if (resultAttack == ResultDe.EchecCritique && resultDefense == ResultDe.RéussiteCritique)
            {
                baseDommage = cible.Attack * 2;
                message.AddSegment($"{attacker.Name} tente une Attack risquée", ConsoleColor.Red)
                    .AddSegment($" mais ", ConsoleColor.Magenta)
                    .AddSegment($"{cible.Name}", ConsoleColor.Red)
                    .AddSegment($" retourne brillamment la situation avec une contre-Attack !", ConsoleColor.Yellow);

                baseDommage = CalculVulnerabilites(baseDommage, typeAttack, attacker, cible, message);

                message.AddSegment($"{cible.Name} ", ConsoleColor.Red)
                    .AddSegment($"inflige ")
                    .AddSegment($"{baseDommage} dégâts! ", ConsoleColor.Magenta)
                    .AddSegment($"à ")
                    .AddSegment($"{attacker.Name} ", ConsoleColor.Blue);

                InfligerDommages(baseDommage, attacker);
                return (0, message);  // Si cet état est atteint, le retour se fait ici
            }

            if (baseDommage < 0) baseDommage = 0;

            ConstruireMessageCombat(message, resultAttack, resultDefense, typeAttack, attacker, cible, Attack, dommageAttack, totalDefense);

            return (baseDommage, message);
        }
        private (int, Message) CalculerVulnerabilitesEtConstruireMessage(int baseDommage, TypeAttack typeAttack, IPersonnage attacker, IPersonnage cible)
        {
            var message = new Message();
            int adjustedDommage = CalculVulnerabilites(baseDommage, typeAttack, attacker, cible, message);
            return (adjustedDommage, message);
        }
        private int CalculVulnerabilites(int dommage, TypeAttack typeAttack, IPersonnage attacker, IPersonnage cible, Message message = null)
        {
            switch (typeAttack)
            {
                case TypeAttack.Sacre:
                    if (cible.TypeDuPersonnage == TypePersonnage.MortVivant)
                    {
                        dommage += attacker.Attack;

                        if (message != null)
                            message.AddSegment($"C'est super efficace ! Dégâts: ", ConsoleColor.Yellow)
                                   .AddSegment($"{dommage}", ConsoleColor.Green);

                    }
                    return dommage;
                case TypeAttack.Normal:
                    return dommage;
                default:
                    Debug.Assert(false, "Type d'Attack inconnu.");
                    return dommage;
            }


        }
        public void InfligerDommages(int dommages, IPersonnage cible)
        {
            var message = new Message();
            message.AddSegment($"{cible.Name}", ConsoleColor.Blue)
                .AddSegment($" perd ")
                .AddSegment($"{dommages}pv \n", ConsoleColor.Red)
                .AddSegment($"{cible.Name} new Life: ");

            if (cible.Life - dommages <= 0)
            {
                message.AddSegment($"0", ConsoleColor.Red);
            }
            else
            {
                message.AddSegment($"{cible.Life - dommages}", ConsoleColor.Green);
            }
            _messageService.Notify(message);
            cible.Life -= dommages;
        }
        private void ConstruireMessageCombat(Message message, ResultDe resultAttack, ResultDe resultDefense, TypeAttack typeAttack, IPersonnage attacker, IPersonnage cible, int Attack, int dommageAttack, int totalDefense)
        {
            message.AddSegment($"{attacker.Name}", ConsoleColor.Blue)
                .AddSegment(" Attack: ").AddSegment($"{resultAttack} ", ConsoleColor.Cyan)
                .AddSegment("contre | ")
                .AddSegment($"{cible.Name}", ConsoleColor.Blue)
                .AddSegment(" Defense:").AddSegment($"{resultDefense}\n ", ConsoleColor.Cyan)
                .AddSegment($"Type de l'Attack : ");

            if (typeAttack == TypeAttack.Sacre)
            {
                message.AddSegment($"{typeAttack}\n", ConsoleColor.Yellow);
            }
            else
            {
                message.AddSegment($"{typeAttack}\n");
            }

            message.AddSegment("Attack Base:")
                .AddSegment($"{Attack}\n", ConsoleColor.Blue)
                .AddSegment("Attack: ")
                .AddSegment($"{dommageAttack}\n", ConsoleColor.Green)
                .AddSegment("Défense: ")
                .AddSegment($"{totalDefense}\n", ConsoleColor.Green);
        }
        private int ApplyMultiplier(int baseValue, int multiplier)
        {
            return baseValue * multiplier;
        }
        private int ResultDefense(ResultDe resultDefense, IPersonnage defenseur)
        {
            int multiplier = resultDefense switch
            {
                ResultDe.EchecCritique => EchecCritiqueMultiplier,
                ResultDe.Echec => EchecCritiqueMultiplier,
                ResultDe.Neutre => NeutreMultiplier,
                ResultDe.Réussite => (int)(NeutreMultiplier * 1.5),
                ResultDe.RéussiteCritique => ReussiteMultiplier,
                _ => defenseur.Defense,
            };

            return ApplyMultiplier(defenseur.Defense, multiplier);
        }
        private int ResultAttack(ResultDe resultAttack, int Attack = -1)
        {
            int multiplier = resultAttack switch
            {
                ResultDe.EchecCritique => EchecCritiqueMultiplier,
                ResultDe.Echec => EchecCritiqueMultiplier,
                ResultDe.Neutre => NeutreMultiplier,
                ResultDe.Réussite => (int)(NeutreMultiplier * 1.5),
                ResultDe.RéussiteCritique => ReussiteMultiplier,
                _ => Attack,
            };
            return ApplyMultiplier(Attack, multiplier);
        }
    }
}
