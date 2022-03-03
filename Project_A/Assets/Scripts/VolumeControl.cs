using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] string volumeName;

    [SerializeField] AudioMixer mixer;
    [SerializeField] Slider slider;
    [SerializeField] float multiplier = 30f;

    void Awake()
    {
        slider.onValueChanged.AddListener(HandleSliderValueChanged);
    }

    void HandleSliderValueChanged(float _value)
    {
        mixer.SetFloat(volumeName, Mathf.Log10(_value) * multiplier);
    }
    
    void Start()
    {
        slider.value = PlayerPrefs.GetFloat(volumeName, slider.value);
    }

    void OnDisable()
    {
        PlayerPrefs.SetFloat(volumeName, slider.value);
    }

   
}
