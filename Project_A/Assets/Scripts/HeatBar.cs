using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    
    public void SetMaxValue(int _heat)
    {
        slider.maxValue = _heat;
        slider.value = 0;
    }

    public void SetValue(int _heat)
    {
        slider.value = _heat;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
    // Update is called once per frame
  
}
