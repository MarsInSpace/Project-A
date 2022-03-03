using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class ReferenceToVFX : MonoBehaviour
{
    [SerializeField]
    private VisualEffect meteorParticles;

    public Rigidbody playerChar;
    Vector3 playerVelocity;
 

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        playerVelocity = playerChar.velocity;
       
        meteorParticles.SetVector3("PlayerVelocity", playerVelocity);
        
    }
}
