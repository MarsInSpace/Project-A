using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{

   public static PopUpText Create(GameObject _prefab, RectTransform _pos, string _text, float _score)
    {
        GameObject _popUpTransform = Instantiate(_prefab, _pos.transform.position, Quaternion.identity);
        PopUpText _popUp = _popUpTransform.GetComponent<PopUpText>();

        _popUp.Setup(_text, _score);
        return _popUp;
    }

    TextMeshProUGUI textMesh;
    float disappearTimer;
    const float DISAPPEAR_TIMER_MAX = 1;
    Color textColor;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();

    }
    void Setup(string _text, float _score)
    {
        textMesh.text = _text + " " + _score.ToString("F0");
        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;
    }

    private void Update()
    {
        float movespeed = 20f;
        transform.position += new Vector3(0, movespeed) * Time.deltaTime;

        if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            float increaseScale = 1;
            transform.localScale += Vector3.one * increaseScale * Time.deltaTime;
        }
        else
        {
            float decreaseScale = 1;
            transform.localScale -= Vector3.one * decreaseScale * Time.deltaTime;
        }


        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            textColor.a -= 3f * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
