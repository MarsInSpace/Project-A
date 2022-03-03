using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Impact : MonoBehaviour
{
    public bool isActive = true;
    [Tooltip("The Impact Damage")]
    public int impactAmount = 1;

    
    private void OnCollisionEnter(Collision collision)
    {
        if (!isActive) return;
        if (!collision.gameObject.CompareTag("Player")) return;
        
        Health health = collision.gameObject.GetComponent<Health>();
        health.ModifyHealth(-impactAmount);
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (!(other.gameObject.CompareTag("Player"))) return;
        
        Health health = other.gameObject.GetComponent<Health>();
        health.ModifyHealth(-impactAmount);
     
    } 
    
}
