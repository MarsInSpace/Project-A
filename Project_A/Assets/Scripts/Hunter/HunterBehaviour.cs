
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class HunterBehaviour : MonoBehaviour
{
    Transform targetTransform;
    Rigidbody targetRigidbody;
    
    [SerializeField] Rigidbody hunterBody;
    public Collider impactCollider;
    [SerializeField] float followSpeed;
    [SerializeField] float respawnDistance = 500f;
    [SerializeField] float minRspawnDist = 500f;


    Vector3 direction;

    void Awake()
    {
        hunterBody = GetComponent<Rigidbody>();
    }

    public void Init(Transform _targetTransform , Rigidbody _targetRigidbody, float _speed, float _respawnDistReduce)
    {
        targetTransform = _targetTransform;
        targetRigidbody = _targetRigidbody;
        followSpeed += _speed;
        respawnDistance -= _respawnDistReduce;
        if (respawnDistance <= minRspawnDist) respawnDistance = minRspawnDist;
        StartInFrontOfTarget();
    }

    void FixedUpdate()
    {
        MoveToTarget();
    }

    public void LookAtTarget()
    {
        transform.LookAt(targetTransform.position); 
    }

    void MoveToTarget()
    {
        if (!targetTransform) return;
        hunterBody.velocity = CalculateDirection(targetTransform) * followSpeed;
    }
    Vector3 CalculateDirection(Transform _targetTransform)
    {
        direction = _targetTransform.position - transform.position;
        direction = direction.normalized;
        return direction;
    }

    void StartInFrontOfTarget()
    {
        transform.position = targetTransform.position + targetRigidbody.velocity.normalized * respawnDistance;
    }

   
}
