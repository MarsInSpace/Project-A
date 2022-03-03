using System;
using System.Collections;
using System.Collections.Generic;
using Coffee.UISoftMask;
using UnityEngine;
using UnityEngine.UI;

public class HunterIndicator : MonoBehaviour
{
    [SerializeField] SoftMask image;
    
    Vector3 defaultPos;
    float defaultAlpha;

    float currentAlpha;
    // Start is called before the first frame update

    void Awake()
    {
        defaultPos = transform.position;
       // secondPos = secondPosTransform.position;
       defaultAlpha = image.alpha;
        currentAlpha = defaultAlpha;

    }

    public void SetAlpha(float _currentDist)
    {
        var _currentRange = (_currentDist / 1000);
        var _newAlpha = 1 - _currentRange;
        if (_newAlpha < .3f)
        {
            _newAlpha = .3f;
        }

        if (_newAlpha >= .8f)
        {
            _newAlpha = 1;
        }

        image.alpha = Mathf.Lerp(image.alpha, _newAlpha, .5f * Time.deltaTime);
    }

    
}
