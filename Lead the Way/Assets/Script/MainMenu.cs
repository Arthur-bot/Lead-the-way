using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class MainMenu : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] GameObject levelSelectionPanel;
    [SerializeField] LevelButton[] levelButtons;
    [SerializeField] AudioSource audioSource2;


    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        keywordActions.Add("Jouer", ShowLevelSelection);
        keywordActions.Add("Musique", Music);
        keywordActions.Add("Menu", GoBack);
        keywordActions.Add("Quitte", LeaveGame);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();
    }

    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        keywordActions[args.text].Invoke();
    }

    void ShowLevelSelection()
    {

        levelButtons[0].UnlockButton();

        for (int i = 0; i < 10; i++)
        {
            levelButtons[i].Init();
            levelButtons[i].UpdateStatus(); //TODO change this method
            levelButtons[i].UpdateLevelImage(); //TODO change this method
        }

        levelSelectionPanel.SetActive(true);
    }

    void Music()
    {
        audioSource2.mute = !audioSource2.mute;
    }

    void GoBack()
    {
        if(levelSelectionPanel.activeSelf == true)
        {
            levelSelectionPanel.SetActive(false);
            for (int i = 0; i < 10; i++)
            {
                levelButtons[i].DisposeKeyword();
            }
        }
    }

    void LeaveGame()
    {
        if(levelSelectionPanel.activeSelf == false)
        {
            Application.Quit();
        }
    }
}
