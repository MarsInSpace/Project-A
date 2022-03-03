using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MotionBlurShaker : MonoBehaviour
{
    public static  MotionBlurShaker Instance {get; private set;}
    
    Volume volume;
    MotionBlur motionBlur;
    float originalIntensity;
    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out motionBlur);
        originalIntensity = motionBlur.intensity.value;
    }

    public void Play(float _intensity, float _duration)
    {
        motionBlur.intensity.Override(_intensity);
        StartCoroutine(Stop(_duration));
    }

    IEnumerator Stop(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        motionBlur.intensity.Override(originalIntensity);
    }
}
