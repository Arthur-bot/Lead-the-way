using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class LevelButton : MonoBehaviour
{
    [Header("Level Infos")]
    [SerializeField] string levelName;
    [SerializeField] string previousLevelName;

    [Header("UI")]
    [SerializeField] Button startButton;
    [SerializeField] GameObject icon;

    bool unlocked;

    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;

    public void Init()
    {
        keywordActions.Add(levelName, EnterLevel);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();
    }

    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
    }

    public void UpdateStatus()
    {
        if (previousLevelName != null && PlayerPrefs.GetInt(previousLevelName) == 1)
        {
            unlocked = true;
        }
    }

    public void UpdateLevelImage()
    {
        if (!unlocked)
        {
            startButton.interactable = false;
            icon.SetActive(false);
        }

        if (unlocked)
        {
            startButton.interactable = true;
            icon.SetActive(true);
        }
    }

    public void EnterLevel()
    {
        if(unlocked)
        {
            DisposeKeyword();
            SceneManager.LoadScene(levelName);
        }

    }

    public void DisposeKeyword()
    {
        keywordRecognizer.Dispose();
        keywordActions.Remove(levelName);
    }

    public void UnlockButton()
    {
        unlocked = true;
    }
}
