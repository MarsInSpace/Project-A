using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_CameraZoom : Feedback
{
    [SerializeField] float zoom;
    [SerializeField] float duration;
    public override void PlayFeedback(Vector3 _position)
    {
        Cinemachine_Manager.Instance.currentMaxZoom += zoom;
        
    }

    IEnumerator StopZoom()
    {
        yield return new WaitForSeconds(duration);
        Cinemachine_Manager.Instance.currentMaxZoom = Cinemachine_Manager.Instance.maxZoom;
    }
}
