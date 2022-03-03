using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_PopUpScore : Feedback
{
    [SerializeField] string[] popUpTexts;
    [SerializeField] PopUp popUpPrefab;
    [SerializeField] float score;
    [SerializeField] bool isForced;

   
    public override void PlayFeedback(Vector3 _position)
    {
        int randomTxt = Random.Range(0, popUpTexts.Length);
        ScoreManager.Instance.RequestPopUp(popUpTexts[randomTxt],score,popUpPrefab, isForced);
    }

  
}
