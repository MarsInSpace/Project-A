using System.Collections;
using UnityEngine;

public class FB_MaterialChange : Feedback
{
    [SerializeField] MeshRenderer target;
    [SerializeField] Material newMaterial;
    [SerializeField] float duration;

    Material defaultMat;
    void Awake()
    {
        defaultMat = target.material;
    }

    public override void PlayFeedback(Vector3 _position)
    {
        if (target)
        {
            target.material = newMaterial;
            StartCoroutine(Stop());
        }
        else
        {
            Debug.LogError("target is empty in " + this + gameObject);
        }
    }

    IEnumerator Stop()
    {
        yield return new WaitForSeconds(duration);
        target.material = defaultMat;
    }
}
