using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FB_Bloom : Feedback
{
    [Range(0,50)]
    [SerializeField] float minIntensity = 0.3f;
    [Range(0,50)]
    [SerializeField] float maxIntensity = 50f;
    [SerializeField] float duration;
    [SerializeField] Color color;
    [SerializeField] bool useRandomColor = true;
    

    public override void PlayFeedback(Vector3 _position)
    {
        if (useRandomColor)
        {
            color = Random.ColorHSV();
        }
        float _intensity = Random.Range(minIntensity, maxIntensity);
        BloomShaker.Instance.Play(_intensity, duration, new Color(color.a,color.g,color.b,100));
    }
}
