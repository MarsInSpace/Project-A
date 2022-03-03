using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] float boostForce;
    [SerializeField] Transform gate;
    [HideInInspector] public Player player;
    [SerializeField] Feedbacks feedbacks;
    private void OnTriggerEnter(Collider _hitInfo)
    {
        if (!_hitInfo.gameObject.CompareTag("Player")) return;
        if (!player)
        {
            player = _hitInfo.GetComponent<Player>();
        }
        
        player.rb.velocity = Vector3.zero;
        player.rb.AddForce(gate.transform.up * boostForce, ForceMode.Impulse);
        player.isInBooster = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        if (!player)
        {
            player = other.GetComponent<Player>();
        }
        feedbacks?.PlayFeedbacks(transform.position);
        player.rb.velocity = Vector3.zero;
        player.rb.AddForce(gate.transform.up * boostForce, ForceMode.Impulse);
        Timing.Instance.DoAfterDelay(delegate { player.isInBooster = false; }, .5f);
    }
    
}
