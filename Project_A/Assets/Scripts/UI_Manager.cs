
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public event dlg_string onRecievedName;
    [Serializable]
    class GameplayPanels
    {
        public GameObject pauseMenu;
        public GameObject pauseFirstButton;
        public GameObject losePanel;
        public GameObject loseFirstButton;
        [Header("Score")]
        public TextMeshProUGUI scoreUI;
        public TextMeshProUGUI currentHighscoreUI;
        public TextMeshProUGUI currentName;

        public UI_InputWindow inputField;
    }

    [SerializeField] GameplayPanels gameplayPanels;
    [SerializeField] HighscoreUI[] highscoreEntries;
    EventSystem currentEvent;
    void Awake()
    {
        currentEvent = EventSystem.current;
        if (gameplayPanels.inputField)
        {
            gameplayPanels.inputField.onPlayerInput += OnReciveName;
        }
        
    }

    void Update()
    {
        if(gameplayPanels.scoreUI)
            gameplayPanels.scoreUI.text = ScoreManager.Instance.currentScore.ToString("F0");
    }

    void Start()
    {
        RegisterEvents();
    }

    void Pause()
    {
        if ( gameplayPanels.pauseMenu != null)
        {
            gameplayPanels.pauseMenu.SetActive(true);
            currentEvent.SetSelectedGameObject( gameplayPanels.pauseFirstButton);
        }
    }

    void Resume()
    {
        if ( gameplayPanels.pauseMenu != null)
        {
            gameplayPanels.pauseMenu.SetActive(false);
        }

    }
    
    void ShowHighscore()
    {
        gameplayPanels.scoreUI.gameObject.SetActive(false);
        gameplayPanels.losePanel?.SetActive(true);
        currentEvent.SetSelectedGameObject( gameplayPanels.loseFirstButton);
        gameplayPanels.currentHighscoreUI.text = ScoreManager.Instance.currentScore.ToString("F0");
    }

    public void ActivateNameInput()
    {
        gameplayPanels.inputField.gameObject.SetActive(true);
        gameplayPanels.inputField.SetName(7);
    }
    public void RegisterEvents()
    {
        GameManager.Instance.onGameover += ShowHighscore;
        GameManager.Instance.onPause += Pause;
        GameManager.Instance.onResume += Resume;
    }

    public void UnregisterEvents()
    {
        GameManager.Instance.onGameover -= ShowHighscore;
        GameManager.Instance.onPause -= Pause;
        GameManager.Instance.onResume -= Resume;

    }

     void OnReciveName(string _text)
    {
        gameplayPanels.currentName.text = _text;
        onRecievedName?.Invoke(_text);
    }
     
     public void UpdateUI (List<HighscoreManager.HighscoreEntry> list) {
         for (int i = 0; i < list.Count; i++) {
             HighscoreManager.HighscoreEntry el = list[i];

             if (el != null && el.score > 0) {
                 // write or overwrite name & points
                 highscoreEntries[i].Setup(el.name, el.score);
             }
         }
     }
    
}
