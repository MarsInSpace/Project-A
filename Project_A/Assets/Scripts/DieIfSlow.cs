using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieIfSlow : MonoBehaviour
{
    float currentSpeed;
    [SerializeField] float dieSpeedThreshold;

    float dieTimer;
    [SerializeField] float dieWaitTime;

    Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        currentSpeed = Player.currentSpeed;
    }

    void Update()
    {
        CheckIfDie();
    }

    void CheckIfDie()
    {
        if (currentSpeed < dieSpeedThreshold)
        {
            dieTimer += Time.deltaTime;
            if (dieTimer >= dieWaitTime)
            {
                health.ModifyHealth(-10);
            }
        }
        else
        {
            dieTimer = 0;
        }
    }
}
