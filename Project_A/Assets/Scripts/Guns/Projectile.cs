using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public event dlg_FloatVectorPlatform onTargetHit;

    [SerializeField] GameObject impactEffect;

    public Rigidbody rb;
    public SphereCollider sphereCollider;
    [SerializeField] float lifeSpan = 5;
    [HideInInspector] public Player owner;

   
    public static Projectile Create(GameObject _projectile, Vector3 _spawnPosition, float _damageAmount, Player _owner)
    {
        Projectile _currentProjectile = Instantiate(_projectile, _spawnPosition, Quaternion.identity).GetComponent<Projectile>();
        _currentProjectile.owner = _owner;
        return _currentProjectile;
      //  _currentProjectile.Setup(enemy, damageAmount);
    }

    private void Update()
    {
        Timing.Instance.DoAfterDelay(delegate { Destroy(gameObject); }, lifeSpan);
    }

    private void OnTriggerEnter(Collider _hitInfo)
    {
        if (_hitInfo.gameObject.CompareTag("Surface"))
        {
            float _dist = (transform.position - owner.transform.position).magnitude;
            Vector2 _dir = (transform.position - owner.transform.position).normalized;
            Platform _platform = _hitInfo.gameObject.GetComponent<Platform>();

            onTargetHit?.Invoke(_dist, _dir, _platform);

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Player _target = collision.gameObject.GetComponent<Player>();
      
        if (collision.gameObject.CompareTag("Surface"))
        {
            float _dist = (transform.position - owner.transform.position).magnitude;
            Vector2 _dir = (transform.position - owner.transform.position).normalized;
            Platform _platform = collision.gameObject.GetComponent<Platform>();

            onTargetHit?.Invoke(_dist, _dir,_platform);

            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
  
}
