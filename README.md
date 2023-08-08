# BattleRoyal-RPG

## Description
**BattleRoyal-RPG** est un jeu de combat tour par tour basé sur la console. Les joueurs contrôlent différents personnages, tels que des zombies et des prêtres, qui s'affrontent dans une arène. La mécanique du jeu repose sur une architecture multithreadée permettant de gérer simultanément des actions comme les attaques et les délais de recharge, rendant le jeu fluide et interactif.

## Caractéristiques

- **Multithreading**: Grâce à l'utilisation de threads, le jeu gère simultanément les attaques, les délais de recharge, et d'autres mécanismes sans que le joueur n'ait à attendre.
- **Système de Classes**: Chaque personnage possède une classe.
- **Compétences**: Chaque classe de personnage possède une ou plusieurs compétences, en plus de l'attaque de base. Une compétence peut avec un type de dégat prédéfini ou modifié en fonction de la classe.

>Exemple : 

| Personnage | Competence | Type |
|------------|:----------:|-----:|
|Prêtre      |AttaqueBase |Sacré |
|Guerrier    |AttaqueBase |Normal|
|Zombie      |AttaqueBase |Normal|

- **Sélection intelligente des cibles**: Les personnages choisissent leurs cibles en fonction de divers critères, garantissant des combats stratégiques.

## Design Pattern utilisé

- **Décorateur**
- **Stratégie**
- **Singleton**

## Utilisation

Pour utiliser le programme:


1. Initialisez vos participants (par exemple, des zombies ou des prêtres).
2. Ajoutez-les à une instance de `BattleArena`.
3. Démarrez le combat avec la méthode `StartBattle`.
4. Lisez les résultats du combat dans la console.

Exemple:
>program.cs
```csharp
var zombie = new Zombie("Zombie Gaetan");
var pretre = new Pretre("Pretre ATesSouhaits");
var arena = new BattleArena(new List<Personnage> { zombie, pretre });

Console.WriteLine("Le combat commence !");
await arena.StartBattle();
Console.WriteLine("Le combat est terminé !");
Console.ReadLine(); // Empêche la console de se fermer immédiatement
```
## Améliorations Futures

- **Système d'Équipe** : Ajout de la mécanique d'équipe basée sur des races ou types de personnages, y compris l'introduction de boss.
- **Armes et Loots** : Intégration d'un système d'armes avec des types d'attaque variés.
- **Amélioration de l'IA** : Développement de l'intelligence de l'IA pour rendre les combats plus dynamiques et imprévisibles.
- **Introduction de Nouvelles Classes de Personnages** : Ajout de diverses nouvelles classes pour enrichir la variété du gameplay.

### Avec Unity :
- **Lien avec Unity** : Intégration du jeu avec Unity pour une expérience utilisateur améliorée.
- **IA Avancée avec Vision et Déplacement** :
  - **Champ de Vision** : Implémentation d'un champ de vision pour l'IA afin qu'elle puisse détecter les ennemis basés sur la ligne de vue.
  - **Algorithme de Déplacement** : Utilisation d'algorithmes de déplacement tels que A* pour une navigation plus réaliste des personnages dans l'arène.
- **Armes et Loots** : Intégration d'un système d'armes avec des types d'attaque variés et des loots d'armes à récupérer dans l'environnement.

