![Main](Main.png)

# Lead the Way

Créé dans le cadre d'un projet scolaire suivant le thème d'un Alt-CTR Gamejam, Lead the way est un jeu de plateforme utilisant des contrôles vocaux. Prisonnié d'un donjon, le joueur devra dicter des ordres au personnage pour que celui ci puisse en sortir.

## Devlog

**Titre :**   Lead the Way <br />
**Début du projet :** 30/04/2021 <br />
**Fin du projet :** 10/05/2021

### Pré-requis pour fonctionner

Pour que la reconnaissance vocale fonctionne il faut :
- Windows 10 avec Cortana d'activé
- Un microphone

### Devlog - 1 : Niveau test et premiers contrôles vocaux


##### Contrôles vocaux : Windows.Speech

Tout d'abord, pour reconnaître les commandes, nous avons besoin de quelques instructions d'utilisations :
- System pour utiliser les Actions
- System.Linq pour les Dictionnaires
- Windows.Speech pour utiliser l'assistant personnel de Windows : **Cortana**

```bash
using System;
using System.Linq;
using UnityEngine.Windows.Speech;
```

Ensuite nous initialisons un Dictionnaire d'Actions avec comme clés les commandes vocales, et le module de reconnaissance vocale.

```bash
Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
KeywordRecognizer keywordRecognizer;
```

Dans la fonction Start de nous définissons nos différents éléments du dictionnaire.

```bash
keywordActions.Add("Start", StartMoving);
keywordActions.Add("Stop", StopMoving);
keywordActions.Add("Droite", MoveRight);
keywordActions.Add("Gauche", MoveLeft);
keywordActions.Add("Hop", Jump);
```

À la suite, nous créons le Keyword Recognizer en précisant ce que l'on veut reconnaître et le niveau de confiance de la reconnaissance (ici low pour reconnaître un maximum de mot venant du joueur plutôt que d'avoir des ordres non reconnus et non éxecutés).

```bash
keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
keywordRecognizer.Start();
```

Puis, nous associons la méthode OnKeywordRecognizer à l'event OnPhraseRecognized

```bash
keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
```
Exemple de méthode :

```bash
void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
{
    keywordActions[args.text].Invoke();
}
```

Et enfin, nous commençons la reconnaissance vocale.

```bash
keywordRecognizer.Start();
```

Dans le cas des contrôle du personnage, les contrôles vocaux sont placés dans le script [Player Controller](https://github.com/Arthur-bot/Lead-the-way/blob/main/Lead%20the%20Way/Assets/Script/PlayerController.cs).

Chaque Action associée dans le dictionary<string, Action> définie une action précise comme les changemets de direction, les sauts, ...
Par exemple :

```bash
    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        else if (wallSliding)
        {
            if (isFacingWall)
                wallJumpingFace = true;
            else
                wallJumping = true;

            Invoke("SetWallJumpingFalse", wallJumpTime);
        }
    }
```

De plus, le script est associé au prefab personnage. Ainsi, à chaque fois que l'un des personnage est détruit, le keyword recognizer qui lui était associé l'est aussi et un nouveau est initialisé après le respawn du personnage.

##### Niveau test

Maintenant que les contrôles sont créés, il ne reste plus qu'une scène pour les tester 

![Niveau 4](Example1.png)

Dans cette scène on voit une étiquette verte 'spawn', d'où le personnage part au début du niveau et où il reapparait après chaque mort, un cercle noir représentant la sortie du niveau et un chemin clair pour y accéder, nous faisant utiliser chaque actions codées précedemment.

### Devlog - 2 : Création d'un menu avec contrôles vocaux

Le jeu en entier doit pouvoir se jouer sans souris ni clavier, il faut donc créer un menu qui puisse être contrôlé par des commandes vocales de la même manière que le personnage. (cf le script [Main Menu](https://github.com/Arthur-bot/Lead-the-way/blob/main/Lead%20the%20Way/Assets/Script/MainMenu.cs)).

Ajout d'un nouveau keyword Recognizer spécifique au menu (les commandes étant inutiles et/ou différentes dans les niveaux).

```bash
keywordActions.Add("Jouer", ShowLevelSelection);
keywordActions.Add("Musique", Music);
keywordActions.Add("Menu", GoBack);
keywordActions.Add("Quitte", LeaveGame);
```

Le menu est composé de 2 pages : 
- la prnicipale avec les différentes options (Jouer, Musique(On/Off), et Quitter)



- la page des niveaux (allant de 1 à 10). Dans celle-ci, les niveaux débloqués et non sont diffénrenciés par leur sprite.
 

### Devlog - 3 : Level Design

Le principal inconvéniant de la reconnaissance vocale est la latence que met le système pour reconnaître chaque commande dite par le joueur, mais aussi le temps de latence entre chaque commande (le joueur ne pouvant pas enchaîner 2 commandes, mais doit laisser un espace entre chaque). Ainsi, il faut pouvoir organiser les niveaux de sorte à laisser du temps au joueur de planifier les prochaines actions sans pour autant enlever du challenge.

Les niveaux ont donc été pensé à cet effet en élargissant le sol. Après un court temps d'adaptation à cette latence, le joueur sera tout à fait capable d'enchaîner les difficultés qui lui feront face.

Pour la création des niveaux, je me suis essentiellement inspiré de 3 jeux de plateforme, Celeste, Super Meat Boy et Catbird.

Il y a maintenant 10 niveaux, dont 3 didactiques permettant au joueur d'apprendre les différentes commandes, puis 7 autres avec une courbe de difficulté croissante. Par exemple, respectivement les niveaux 4 et 7.

![Niveau 4](Example1.png)
![Niveau 7](Example2.png)


### Devlog - 4 : Polissage (son, animation UI, ...)

Cette fois, moins de code et plus d'assets :
- Ajout d'une transition dynamique entre les scènes
- Ajout d'une musique (différente entre le menu et les niveaux)
- Ajout de son pour les sauts (lorsque l'on atterri) et la mort du personnage.
- Ajout d'options pour le son et musique (On/Off) -> problème non résolu, le paramètre ne reste pas entre les niveaux.
- Ajout d'un menu 'Pause' dans les niveaux pour régler la musique, le son ou alors pour retourner au menu; Celui-ci fonctionnant par contrôle vocaux, il fallait créer de nouveaux ordres : 

```bash
keywordActions.Add("Pause", PauseGame);
keywordActions.Add("Musique", Music);
keywordActions.Add("Son", Sound);
keywordActions.Add("Menu", GoToMenu);
```

- 

### Devlog - 5 : Pistes d'amélioration

- Ajouter de nouvelles plateformes (Mac, ios, android, ...). Actuellement, la reconnaissance vocale ne fonctionne que sur Windows 10 (étant obliger de passer par Cortana), cependant on pourrait tout aussi bien passer par d'autres logiciels de reconnaissance vocale pour élargir le nombre de plateformes.
- Ajouter une fonction permettant de capter l'amplitude de la voix, ainsi, en utilisant le commande pour sauter ('Hop'), plus le joueur le dit fort,plus le personnage sautera haut et inversement si le joueur le dit faiblement.
- Ajouter un autre objectif pour améliorer le gameplay. À la manière des fraises dans Celeste, rajouter un collectionable présent dans certains niveaux pour donner un nouvel objectif au joueur, tout en rajoutant du défi.
- Ajouter d'autres niveaux, avec des architectures différentes et des mécaniques spécifiques à ces niveaux (niveau dans l'espace avec un persnnage ralenti, pouvant sauter plus haut et plus loin pour simuler la différence de gravité, ...)
- Corriger certains bugs comme la musique et le son qui ne gardent pas le paramètre choisi correctement, le personnage qui peut rester bloqué sur un rebord après un saut, ...

## Licence
Le contenu de ce projet est licencié sous la licence  GNU GENERAL PUBLIC, sauf si une autre est spécifiée plus haut. Voir [LICENCE file](https://github.com/Arthur-bot/Lead-the-way/blob/main/LICENSE) dans le projet pour plus d'informations.
