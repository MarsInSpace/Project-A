using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_ChangePitch : Feedback
{
    [SerializeField] float pitchValue;
    [SerializeField] float duration;

   public override void PlayFeedback(Vector3 _position)
   {
       StartCoroutine(AudioManager.Instance.ChangePitch(pitchValue, duration));
   }
}
