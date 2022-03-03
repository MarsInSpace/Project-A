using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DistortionShaker : MonoBehaviour
{
    public static  DistortionShaker Instance {get; private set;}
    
    Volume volume;
    LensDistortion lensDistortion;
    float originalIntensity;
    Vector2 originalCenter;
    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out lensDistortion);
        originalIntensity = lensDistortion.intensity.value;
        originalCenter = lensDistortion.center.value;
    }

    public void Play(float _intensity, float _duration, Vector2 _center)
    {
        lensDistortion.center.value = _center;
        lensDistortion.intensity.Override(_intensity);
        StartCoroutine(Stop(_duration));
    }

    IEnumerator Stop(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        lensDistortion.intensity.Override(originalIntensity);
        lensDistortion.center.value = originalCenter;
    }
}
