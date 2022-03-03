using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_VibrationPulse : Feedback
{
    [SerializeField] float lowStart = .3f;
    [SerializeField] float highIntensity = .5f;
    [SerializeField] float burstTime = .2f;


    [SerializeField] float duration = 1f;
    
    
    public override void PlayFeedback(Vector3 _position)
    {
        Rumbler.Instance.RumblePulse(lowStart, highIntensity, burstTime,duration);
        
        StartCoroutine(Stop());
        
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(duration);
        Rumbler.Instance.StopRumble();
        
    }
}
