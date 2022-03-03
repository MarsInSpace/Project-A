using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BloomShaker : MonoBehaviour
{
    public static BloomShaker Instance {get; private set; }

    Volume volume;
    Bloom bloom;

    Color originalColor;
    float originalIntensity;
    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out bloom);

        originalIntensity = bloom.intensity.value;
        originalColor = bloom.tint.value;
    }

    public void Play(float _intensity, float _duration, Color _color)
    {
        bloom.intensity.Override(_intensity);
        bloom.tint.value = _color;
        StartCoroutine(Stop(_duration));
    }

    IEnumerator Stop(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        bloom.intensity.Override(originalIntensity);
        bloom.tint.value = originalColor;
    }
}
