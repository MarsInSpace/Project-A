using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class FB_Distortion : Feedback
{

    [Range(-1,1)]
    [SerializeField] float minIntensity;
    [Range(-1,1)]
    [SerializeField] float maxIntensity;
    [SerializeField] float duration;
    
  

    public override void PlayFeedback(Vector3 _position)
    {
        float _intensity = Random.Range(minIntensity, maxIntensity);
        DistortionShaker.Instance.Play(_intensity,duration, _position);
    }
}
