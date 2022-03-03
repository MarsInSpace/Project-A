using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCamera : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    void Update()
    {
        transform.position = playerTransform.position;
    }
}
