using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GrappleTargetHelper : MonoBehaviour
{
    Collider parentCollider;
    public float sizeOffset = 1.3f;
    Collider currentCollider;
    private void Awake()
    {
        SetCollider();
    }
  
    void SetCollider()
    {
        parentCollider = GetComponentInParent<Collider>();

        if (parentCollider.GetType() == typeof(MeshCollider))
        {
            MeshCollider parentMesh = parentCollider.GetComponent<MeshCollider>();
            MeshCollider myMeshCollider = gameObject.AddComponent<MeshCollider>();
            myMeshCollider.sharedMesh = parentMesh.sharedMesh;
            myMeshCollider.convex = parentMesh.convex;
            myMeshCollider.isTrigger = true;
            transform.localScale *= sizeOffset;
            currentCollider = myMeshCollider;
        }
        else if (parentCollider.GetType() == typeof(BoxCollider))
        {
            BoxCollider _parentBox = parentCollider.GetComponent<BoxCollider>();
            BoxCollider _myBox = gameObject.AddComponent<BoxCollider>();
            _myBox.center = _parentBox.center;
            _myBox.size = new Vector3(_parentBox.size.x + sizeOffset, _parentBox.size.y + sizeOffset,_parentBox.size.z);
            _myBox.isTrigger = true;
          //  transform.localScale = new Vector3(transform.localScale.x + sizeOffset, transform.localScale.y + sizeOffset, transform.localScale.z + sizeOffset);
            currentCollider = _myBox;
        }
        else if (parentCollider.GetType() == typeof(SphereCollider))
        {
            SphereCollider _parentSphere = parentCollider.GetComponent<SphereCollider>();
            SphereCollider _mySphere = gameObject.AddComponent<SphereCollider>();
            _mySphere.center = _parentSphere.center;
            _mySphere.radius = _parentSphere.radius * sizeOffset;
            _mySphere.isTrigger = true;
            currentCollider = _mySphere;

        }
        else if (parentCollider.GetType() == typeof(CapsuleCollider))
        {
            CapsuleCollider _parentCapsule = parentCollider.GetComponent<CapsuleCollider>();
            CapsuleCollider _myCapsule = gameObject.AddComponent<CapsuleCollider>();
            _myCapsule.center = _parentCapsule.center;
            _myCapsule.direction = _parentCapsule.direction;
            _myCapsule.height = _parentCapsule.height;
            _myCapsule.radius = _parentCapsule.radius * sizeOffset;
            _myCapsule.isTrigger = true;
            currentCollider = _myCapsule;

        }
    }

   public Vector3 GetClosestPoint(Vector3 _point)
    {
        Vector3 _closestPoint = parentCollider.ClosestPoint(_point);
        return _closestPoint;
    }
}
