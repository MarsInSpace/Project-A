using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    [SerializeField] int maxCount = 3;
    [SerializeField] UI_Manager uiManager;

    
    void Awake()
    {
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);
        
        
        if (highscores == null) {
            // There's no stored table, initialize
            Debug.Log("Initializing table with default values...");
            AddHighscoreEntry(0, "CMK");
            AddHighscoreEntry(0, "JOE");
            AddHighscoreEntry(0, "DAV");
    
            // Reload
            jsonString = PlayerPrefs.GetString("highscoreTable");
            highscores = JsonUtility.FromJson<Highscores>(jsonString);
        }
        
        SortHighscores(highscores);
        uiManager.UpdateUI(highscores.highscoreEntryList);


    }
    
    public void AddHighscoreEntry(int _score, string _name) {
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = _score, name = _name };
        
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }
        
        for (int i = 0; i < maxCount; i++) {
            if (i >=   highscores.highscoreEntryList.Count || highscoreEntry.score >=   highscores.highscoreEntryList[i].score) {
                // add new high score
                highscores.highscoreEntryList.Insert (i, highscoreEntry);

                while ( highscores.highscoreEntryList.Count > maxCount) {
                    highscores.highscoreEntryList.RemoveAt (maxCount);
                }

                SaveHighscore(highscores);
                
                uiManager.UpdateUI(highscores.highscoreEntryList);
                break;
            }
        }

        // // Add new entry to Highscores
        // highscores.highscoreEntryList.Add(highscoreEntry);
        //
        // // Save updated Highscores
        // SaveHighscore(highscores);
    
    }

    void SaveHighscore(Highscores _highscores)
    {
        string json = JsonUtility.ToJson(_highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    void SortHighscores(Highscores _highscores)
    {
        for (int i = 0; i < _highscores.highscoreEntryList.Count; i++) {
            for (int j = i + 1; j < _highscores.highscoreEntryList.Count; j++) {
                if (_highscores.highscoreEntryList[j].score > _highscores.highscoreEntryList[i].score) {
                    // Swap
                    (_highscores.highscoreEntryList[i], _highscores.highscoreEntryList[j]) = (_highscores.highscoreEntryList[j], _highscores.highscoreEntryList[i]);
                }
            }
        }
    }
    
   
    
    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }
    
    [System.Serializable] 
    public class HighscoreEntry {
        public int score;
        public string name;
    }

}
