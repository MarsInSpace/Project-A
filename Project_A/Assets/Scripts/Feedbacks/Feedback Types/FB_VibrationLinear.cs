using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_VibrationLinear : Feedback
{
    [SerializeField] float lowStart = .1f;
    [SerializeField] float lowEnd = .3f;
    [SerializeField] float highStart = .4f;
    [SerializeField] float highEnd = .6f;


    [SerializeField] float duration = 0.2f;
    
    
    public override void PlayFeedback(Vector3 _position)
    {
        Rumbler.Instance.RumbleLinear(lowStart, lowEnd, highStart,highEnd,duration);
        
        StartCoroutine(Stop());
        
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(duration);
        Rumbler.Instance.StopRumble();
        
    }
}
