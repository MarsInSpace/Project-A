using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event dlg_VoidNoArg onDie;
    [SerializeField] float maxHealth;
    [SerializeField] Feedbacks deathEffect;

    float currentHealth;
    // Start is called before the first frame update
  
    public void ModifyHealth(int _amount)
    {
        currentHealth += _amount;
        if (currentHealth > 0) return;
        Die();
    }

    void Die()
    {
        deathEffect?.PlayFeedbacks(transform.position);
        Destroy(gameObject);
        Timing.Instance.DoAfterDelay(delegate {onDie?.Invoke(); }, 2f);
    }
}
