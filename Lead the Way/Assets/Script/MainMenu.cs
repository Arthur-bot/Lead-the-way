using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class MainMenu : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject levelSelectionPanel;
    [SerializeField] LevelButton[] levelButtons;
    [SerializeField] GameObject optionsPanel;

    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        keywordActions.Add("Jouer", ShowLevelSelection);
        keywordActions.Add("Options", ShowOptions);
        keywordActions.Add("Back", GoBack);
        keywordActions.Add("Quitte", LeaveGame);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();
    }

    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
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

    void ShowOptions()
    {
        optionsPanel.SetActive(true);
    }

    void GoBack()
    {
        if(optionsPanel.activeSelf == true)
        {
            optionsPanel.SetActive(false);
        }
        else if(levelSelectionPanel.activeSelf == true)
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
        if(optionsPanel.activeSelf == false && levelSelectionPanel.activeSelf == false)
        {
            Application.Quit();
        }
    }
}
