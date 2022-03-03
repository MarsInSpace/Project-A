using System.Collections;
using UnityEngine;

public class FB_TimeManipulation : Feedback
{
    [SerializeField] float timeScale = .5f;
    [SerializeField] float duration = 1f;


    public override void PlayFeedback(Vector3 _position)
    {
        Time.timeScale = timeScale;
        StartCoroutine(Stop());
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(duration);
        Time.timeScale = 1;
    }
}
