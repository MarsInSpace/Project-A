using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreTxt;
    [SerializeField] TextMeshProUGUI comboTxt;

    public void Setup(string _text, float _score, RectTransform _position)
    {
        scoreTxt.text = _score.ToString("f0");
        comboTxt.text = _text;
        transform.position = _position.transform.position;
        StartCoroutine(GetDestroyed());
    }

    IEnumerator GetDestroyed()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
} 
