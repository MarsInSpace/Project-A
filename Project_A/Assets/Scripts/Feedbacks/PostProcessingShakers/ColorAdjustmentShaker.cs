using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ColorAdjustmentShaker : MonoBehaviour
{
    public static ColorAdjustmentShaker Instance {get; private set; }

    Volume volume;
    ColorAdjustments colorAdjustments;

    Color originalColor;
    float originalSaturation;
    float originalContrast;
    float originalHue;
    float originalPostExposure;
    void Awake()
    {
        Instance = this;
        volume = GetComponent<Volume>();
        volume.profile.TryGet(out colorAdjustments);

        originalPostExposure = colorAdjustments.postExposure.value;
        originalColor = colorAdjustments.colorFilter.value;
        originalContrast = colorAdjustments.contrast.value;
        originalHue = colorAdjustments.hueShift.value;
        originalSaturation = colorAdjustments.saturation.value;
    }

    public void Play(float _postExp, float _contrast, float _hue,float _saturation, Color _color, float _duration)
    {
        SetValues(_postExp, _contrast,_hue,_saturation,_color);
        StartCoroutine(Stop(_duration));
    }

    IEnumerator Stop(float _duration)
    {
        yield return new WaitForSeconds(_duration);
         SetValues(originalPostExposure, originalContrast,originalHue,originalSaturation,originalColor);
    }

    void SetValues(float _postExp, float _contrast, float _hue,float _saturation, Color _color)
    {
        colorAdjustments.postExposure.Override(_postExp);
        colorAdjustments.colorFilter.Override(_color);
        colorAdjustments.contrast.Override(_contrast);
        colorAdjustments.hueShift.Override(_hue);
        colorAdjustments.saturation.Override(_saturation);
    }
}
