using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeatUI : MonoBehaviour
{
    Image image;
    [SerializeField] Sprite emptySprite;
    [SerializeField] Sprite fullSprite;
    [SerializeField] float speed = 5;
    [SerializeField] float scaleMultiplier = 1.35f;



    [SerializeField] Color activeColor;
    [SerializeField] Color heatedColor;

    bool growing;
    [HideInInspector]public bool isHeated;

    Vector3 defaultScale;
    Vector3 activeScale;
    Color defaultColor;
    
    void Awake()
    {
        image = GetComponent<Image>();
        defaultScale = transform.localScale;
        activeScale = defaultScale * scaleMultiplier;
        defaultColor = image.color;
        
    }
    
    void Update()
    {
        if (growing && image.sprite == fullSprite)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, activeScale, .8f * speed* Time.deltaTime);
        }
        else
        {
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, .8f * speed * Time.deltaTime);
        }

        if (isHeated)
        {
            image.sprite = emptySprite;
            image.color = new Color(heatedColor.r, heatedColor.g, heatedColor.b);
        }
    }

    public void Activate()
    {
        image.sprite = fullSprite;
        if(!isHeated)
         image.color = new Color(activeColor.r, activeColor.g, activeColor.b);
    }

    public void deactive()
    {
        ResetColor();
        image.sprite = emptySprite;
    }

    public void SetGrowing(bool isGrowing)
    {
        growing = isGrowing;
    }
    public void ResetColor()
    {
        if (!isHeated)
        {
            image.color = new Color(defaultColor.r, defaultColor.g, defaultColor.b);
        }
    }
    
    
}
