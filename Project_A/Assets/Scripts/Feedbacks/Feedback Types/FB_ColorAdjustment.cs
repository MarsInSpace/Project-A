using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FB_ColorAdjustment : Feedback
{
    [SerializeField] float postExposure;
    [Space(3)]
    [Range(-100,100)]
    [SerializeField] float minContrast;
    [Range(-100,100)]
    [SerializeField] float maxContrast;
    [Space(3)]
    [Range(-180,180)]
    [SerializeField] float minHueShift;
    [Range(-180,180)]
    [SerializeField] float maxHueShift;
    [Space(3)]
    [Range(-100,100)]
    [SerializeField] float minSaturation;
    [Range(-100,100)]
    [SerializeField] float maxSaturation;
    [SerializeField] Color colorFilter;
    [SerializeField] bool useRandomColor;
    [SerializeField] float duration;


  
    public override void PlayFeedback(Vector3 _position)
    {
        if (useRandomColor)
        {
            colorFilter = Random.ColorHSV();
        }
        ColorAdjustmentShaker.Instance.Play(postExposure, Random.Range(minContrast,maxContrast),Random.Range(minHueShift,maxHueShift),Random.Range(minSaturation,maxSaturation),new Color(colorFilter.a,colorFilter.g,colorFilter.b,100),duration);
    }
}
