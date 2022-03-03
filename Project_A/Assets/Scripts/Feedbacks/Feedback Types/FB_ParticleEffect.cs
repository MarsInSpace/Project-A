using System.Collections;
using UnityEngine;

public class FB_ParticleEffect : Feedback
{
    [Tooltip("Add different particle Types in ParticlePools under Managers")]
    [SerializeField] ParticleType particleType;
    [SerializeField] float lifeTime = 1.5f;
   
    public override void PlayFeedback(Vector3 _position)
    {
        GameObject _obj = ParticlePools.Instance.GetFromPool(particleType.ToString(), _position);
        StartCoroutine(SendToPool(_obj));
    }

    IEnumerator SendToPool(GameObject _obj)
    {
        yield return new WaitForSeconds(lifeTime);
        ParticlePools.Instance.AddToPool(particleType.ToString(), _obj);
    }
}
