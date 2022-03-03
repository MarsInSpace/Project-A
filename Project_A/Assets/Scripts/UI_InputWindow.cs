using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InputWindow : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    public event dlg_string onPlayerInput;

    // Start is called before the first frame update
    void Awake()
    {
        Hide();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            GetPlayerName();
            Hide();
        }
    }

     void GetPlayerName()
    {
        onPlayerInput?.Invoke(inputField.text);
    }
    public void SetName(int _characterLimit)
    {
        gameObject.SetActive(true);
        inputField.Select();
        inputField.characterLimit = _characterLimit;
    }

     void Hide()
    {
        gameObject.SetActive(false);
    }

   
}
