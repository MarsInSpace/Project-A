using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FB_ChromaticAberation : Feedback
{
    [Range(0,1)]
    [SerializeField] float minIntensity = 0.5f;
    [Range(0,1)]
    [SerializeField] float maxIntensity = 1f;
    [SerializeField] float duration;
 
    public override void PlayFeedback(Vector3 _position)
    {
        float _intensity = Random.Range(minIntensity, maxIntensity);
        ChromaticShaker.Instance.Play(_intensity, duration);
        
    }
}
