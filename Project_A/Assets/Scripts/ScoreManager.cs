using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    [SerializeField] TextMeshProUGUI multiplierUI;
    [SerializeField] RectTransform popUpPos;
    public Canvas canvas;
    static bool canPop;
    [SerializeField] float comboTime;
    public int currentScore;
    public int maxMultiplier = 5;
    int currentMultiplier;
    float multiplierResetTimer;

    private void Awake()
    {
        Instance = this;
        currentScore = 0;
        currentMultiplier = 1;
        canPop = true;
    }

    private void Update()
    {
        if (currentMultiplier > 1)
        {
            multiplierResetTimer += Time.deltaTime;
            if (multiplierResetTimer >= comboTime)
            {
                multiplierResetTimer = 0;
                currentMultiplier = 1;
            }
            multiplierUI.gameObject.SetActive(true);
            multiplierUI.text = "X" + currentMultiplier;
        }
        else
        {
            multiplierUI.gameObject.SetActive(false);
        }
    }

    public void AddScore(int _score)
    {
        multiplierResetTimer = 0;
        currentScore += _score * currentMultiplier;
    }

    public void AddMultiplier()
    {
        currentMultiplier += 1;
        if (currentMultiplier > maxMultiplier)
        {
            currentMultiplier = maxMultiplier;
        }
    }

    public void RequestPopUp(string _txt, float _score, PopUp _prefab, bool isForced = false)
    {
        if (!canPop && !isForced) return;
        AddPopUp(_txt, _score, _prefab);
        canPop = false;
        StartCoroutine(RequestToAdd());
    }
    
    IEnumerator RequestToAdd()
    {
        yield return new WaitForSeconds(3);
        canPop = true;
    }
    void AddPopUp(string _text, float _score, PopUp _prefab)
    {
        if (!_prefab) return;
        
        PopUp _popUp = Instantiate(_prefab);
        _popUp.Setup(_text,_score,popUpPos);
        _popUp.transform.SetParent(canvas.transform);
      
    }

   
   
}
