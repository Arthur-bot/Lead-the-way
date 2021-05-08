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

### Devlog - 1 : Création d'une première scène et des premiers contrôles vocaux

```bash
using System;
using System.Linq;
using UnityEngine.Windows.Speech;
```


```bash
Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
KeywordRecognizer keywordRecognizer;
```


```bash
keywordActions.Add("Start", StartMoving);
keywordActions.Add("Stop", StopMoving);
keywordActions.Add("Droite", MoveRight);
keywordActions.Add("Gauche", MoveLeft);
keywordActions.Add("Hop", Jump);
```

```bash
keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray(), ConfidenceLevel.Low);
keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
keywordRecognizer.Start();
```

```bash
void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
{
    keywordActions[args.text].Invoke();
}
```

### Devlog - 2 : Création d'un menu avec contrôles vocaux



### Devlog - 3 : Level Design et affinage des contrôles

![Example1](Example1.png)
![Example2](Example2.png)

### Devlog - 4 : Polissage (son, animation UI, ...)

## Licence
Le contenu de ce projet est licencié sous la licence  GNU GENERAL PUBLIC, sauf si une autre est spécifiée plus haut. Voir [LICENCE file](https://github.com/Arthur-bot/Lead-the-way/blob/main/LICENSE) dans le projet pour plus d'informations.
