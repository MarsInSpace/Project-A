using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticShaker : MonoBehaviour
{
    public static ChromaticShaker Instance {get; private set; }

    Volume volume;
    ChromaticAberration chromaticAberration;
    float originalIntensity;
    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out chromaticAberration);
        originalIntensity = chromaticAberration.intensity.value;
    }

    public void Play(float _intensity, float _duration)
    {
        chromaticAberration.intensity.Override(_intensity);
        StartCoroutine(Stop(_duration));
    }

    IEnumerator Stop(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        chromaticAberration.intensity.Override(originalIntensity);
    }
}
