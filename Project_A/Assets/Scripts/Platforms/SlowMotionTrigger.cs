
using UnityEngine;

public class SlowMotionTrigger : MonoBehaviour
{
    [SerializeField] Feedbacks slowMotionFeedbacks;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
         slowMotionFeedbacks?.PlayFeedbacks(transform.position);
    }
}
