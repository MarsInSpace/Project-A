using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_MotionBlur : Feedback
{
    [Range(-1,1)]
    [SerializeField] float minIntensity;
    [Range(-1,1)]
    [SerializeField] float maxIntensity;
    [SerializeField] float duration;

    public override void PlayFeedback(Vector3 _position)
    {
        float _intensity = Random.Range(minIntensity, maxIntensity);
        MotionBlurShaker.Instance.Play(_intensity,duration);
    }
}
