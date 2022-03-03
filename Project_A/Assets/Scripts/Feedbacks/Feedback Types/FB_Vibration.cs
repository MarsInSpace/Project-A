using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Vibration : Feedback
{
    [SerializeField] float lowIntensity = .3f;
    [SerializeField] float highIntensity = .6f;

    [SerializeField] float duration = 0.2f;
    
    
    public override void PlayFeedback(Vector3 _position)
    {
        Rumbler.Instance.RumbleConstant(lowIntensity, highIntensity, duration);
        
        StartCoroutine(Stop());
        
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(duration);
        Rumbler.Instance.StopRumble();
        
    }


}
