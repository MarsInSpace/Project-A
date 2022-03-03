using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class GameManager : MonoBehaviour
{
    public event dlg_VoidNoArg onGameover;
    public event dlg_VoidNoArg onPause;
    public event dlg_VoidNoArg onResume;


    public static GameManager Instance; 
    public static bool isPaused;
    [Header("Managers")]
    [SerializeField] Player player;
    [SerializeField] GridGenerator grid;
    [SerializeField] HunterSpawner hunterSpawner;
    [SerializeField] UI_Manager uiManager;
    [SerializeField] HighscoreManager highscoreManager;
    
    
    [Header("background music")]
    [SerializeField] bool music;
    [SerializeField] AudioSource bgAudioSource;
    [SerializeField] AudioClip loopClip;
  
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        isPaused = false;

        StartCoroutine(ChangeBackgroundMusic());

        InputHandler.Instance.RegisterEvents();
        InputHandler.Instance.onPause += CheckPauseInput;
        
        if (player)
        {
            player.Init();
            player.onDie += GetPlayerName;
        }
        else
            Debug.LogWarning("Player is empty in Game Manager Inspector");
        
        if (grid)
            grid.Init();
        else
            Debug.LogWarning("Grid Manager is empty in Game Manager Inspector");
        if (uiManager)
        {
            uiManager.RegisterEvents();
            uiManager.onRecievedName += GameOver;
        }
        else
        {
            Debug.LogWarning("UI Manager is empty in Game Manager Inspector");
        }

    }

    IEnumerator ChangeBackgroundMusic()
    {
        yield return new WaitForSeconds(bgAudioSource.clip.length);
        bgAudioSource.clip = loopClip;
        bgAudioSource.loop = true;
        bgAudioSource.Play();
    }

    private void Update()
    {
        Timing.Instance.Run();
        if (isPaused) return;
        if (!player) return;
        
        player.Run();
        
        if (grid)
         grid.Run();
        
        if (hunterSpawner)
            hunterSpawner.Run();

    }

    private void FixedUpdate()
    {
        if (isPaused) return;
       
        if (player != null)
        {
            player.FixedRun();

        }
    }
    void Pause()
    {
        onPause?.Invoke();
        Time.timeScale = 0;
        isPaused = true;
    }

     void CheckPauseInput()
    {
        Debug.Log("yay");
        if (isPaused)
        {
            Debug.Log("is Resuming, isPaused: " + isPaused);
            Resume();
        }
        else
        {
            Debug.Log("is pausing, isPaused: " + isPaused);
            Pause();
        }
    }

    public void Resume()
    {
        onResume?.Invoke();
        Time.timeScale = 1;
        isPaused = false;

    }
    
    public void Restart()
    {
        GamePad.SetVibration(0, 0, 0);
        InputHandler.Instance.UnregisterEvents();
        uiManager.UnregisterEvents();
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void LoadScene(int _sceneNumber)
    {
        InputHandler.Instance.UnregisterEvents();
        SceneManager.LoadScene(_sceneNumber);
    }

    void GetPlayerName()
    {
        isPaused = true;
        Time.timeScale = 0;
        AudioManager.Instance.ResetPitch();
        Rumbler.Instance.StopRumble();

        uiManager.ActivateNameInput();
    }
    
    void GameOver(string _playerName)
    {
        onGameover?.Invoke();
        InputHandler.Instance.UnregisterEvents();
        highscoreManager.AddHighscoreEntry(ScoreManager.Instance.currentScore, _playerName); 
    }

    public void DeleteSaves()
    {
        PlayerPrefs.DeleteAll();
    }
}

public delegate void dlg_VoidNoArg();
public delegate void dlg_ChunkIndex(Vector3 _index);
public delegate void dlg_FloatVectorPlatform(float _dist, Vector2 _dir, Platform _platform);

public delegate void dlg_string(string _text);


