using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class LevelManager : MonoBehaviour
{

    #region Singleton
    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    #endregion

    [Header("Level Specifications")]
    [SerializeField] Transform spawn;
    [SerializeField] string levelName;
    [SerializeField] string nextLevel;


    [Header("Player")]
    [SerializeField] GameObject playerPrefabs;
    PlayerController currentPlayer;


    Dictionary<string, Action> keywordActions = new Dictionary<string, Action>();
    KeywordRecognizer keywordRecognizer;

    // Start is called before the first frame update
    void Start()
    {
        keywordActions.Add("Menu", GoToMenu);
        //keywordActions.Add("Restart", RestartLevel);

        keywordRecognizer = new KeywordRecognizer(keywordActions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += OnKeywordRecognizer;
        keywordRecognizer.Start();

        Init();
    }
    void OnKeywordRecognizer(PhraseRecognizedEventArgs args)
    {
        Debug.Log("Keyword: " + args.text);
        keywordActions[args.text].Invoke();
    }

    void Init()
    {
        SpawnPlayer();
    }

    public void RestartLevel()
    {
        DeathPlayer();
        SpawnPlayer();
    }

    public void FinishLevel()
    {
        if(nextLevel != null)
        {
            PlayerPrefs.SetInt(levelName, 1);
            SceneManager.LoadScene(nextLevel);
        }
        else
        {
            GoToMenu();
        }
    }

    void SpawnPlayer()
    {
        currentPlayer = Instantiate(playerPrefabs, spawn.position, Quaternion.identity).GetComponent<PlayerController>();
    }

    void DeathPlayer()
    {
        Destroy(currentPlayer.gameObject);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }
}
