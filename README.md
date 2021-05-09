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
- Un bon microphone

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

Dans le cas des contrôle du personnage, les contrôles vocaux sont placés dans le script [Player Controller](https://github.com/Arthur-bot/Lead-the-way/blob/main/LICENSE).

Chaque Action associée dans le dictionary<string, Action> définie une action précise comme les changemets de direction, les sauts, ...
Par exemple :

```bash
keywordRecognizer.Start();
```

De plus, le script est associé au prefab personnage. Ainsi, à chaque fois que l'un des personnage est détruit, le keyword recognizer qui lui était associé l'est aussi et un nouveau est initialisé après le respawn du personnage.

### Devlog - 2 : Création d'un menu avec contrôles vocaux

Le jeu en entier doit pouvoir se jouer sans souris ni clavier, il faut donc créer un menu qui puisse être contrôlé par des commandes vocales de la même manière que le personnage. (cf le script [Main Menu](https://github.com/Arthur-bot/Lead-the-way/blob/main/Lead%20the%20Way/Assets/Script/MainMenu.cs).
Ajout d'un nouveau keyword Recognizer spécifique au menu (les commandes étant inutiles et/ou différentes dans les niveaux).

```bash
keywordActions.Add("Jouer", ShowLevelSelection);
keywordActions.Add("Musique", Music);
keywordActions.Add("Menu", GoBack);
keywordActions.Add("Quitte", LeaveGame);
```

### Devlog - 3 : Level Design

Le principal inconvéniant de la reconnaissance vocale est la latence que met le système pour reconnaître chaque commande dite par le joueur. Ainsi, il faut pouvoir organiser les niveaux de sorte à laisser du temps au joueur pour anticiper les prochaines actions sans pour autant enlever du challenge.

Les niveaux ont donc été pensé pour laisser le temps au joueur de réagir aux difficultés de chaque niveau. À l'instar d'un 

![Example1](Example1.png)
![Example2](Example2.png)

### Devlog - 4 : Polissage (son, animation UI, ...)

## Licence
Le contenu de ce projet est licencié sous la licence  GNU GENERAL PUBLIC, sauf si une autre est spécifiée plus haut. Voir [LICENCE file](https://github.com/Arthur-bot/Lead-the-way/blob/main/LICENSE) dans le projet pour plus d'informations.
