using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class UIController : MonoBehaviour
{

    [SerializeField] GameObject pausePanel;

    bool isPaused;

    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;


    void Start()
    {
        keywordActions.Add("Pause", PauseGame);
        keywordActions.Add("Musique", Music);
        keywordActions.Add("Son", Sound);
        keywordActions.Add("Menu", GoToMenu);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();
    }
    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        keywordActions[args.text].Invoke();
    }

    void PauseGame()
    {
        if(!isPaused)
        {
            pausePanel.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
        else
        {
            pausePanel.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    void GoToMenu()
    {
        if(isPaused)
        {
            pausePanel.SetActive(false);
            LevelManager.Instance.GoToMenu();
        }
    }

    void Sound()
    {
        if(isPaused)
        {
            UserInterfaceAudio.Instance.SFX();
        }
    }

    void Music()
    {
        if (isPaused)
        {
            UserInterfaceAudio.Instance.Music();
        }
    }

}
