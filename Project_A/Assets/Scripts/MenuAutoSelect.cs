using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuAutoSelect : MonoBehaviour
{
    [SerializeField] Button firstSelection;
    void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(firstSelection.gameObject);
        firstSelection.Select();
    }
    
}
