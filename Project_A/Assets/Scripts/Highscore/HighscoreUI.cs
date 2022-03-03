using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HighscoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI scoreText;

    public void Setup(string _name, int _score)
    {
        nameText.text = _name;
        scoreText.text = _score.ToString();
    }
}
