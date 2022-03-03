
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Dash_Gun : Gun
{
    [Header("Air-Push variables")]
    [SerializeField] GameObject dashEffect;

    [SerializeField] Feedbacks dashFeedbacks;
    [SerializeField] Feedbacks overheatFeedbacks;   
    [SerializeField] Feedbacks overheatPulsVibration;

    [SerializeField] float dashEffectDuration;
    float dashEffectTimer;

    [SerializeField] float dashForce;
    [SerializeField] float isInDangerForce;

    [Header("Heating")]
    [Tooltip("the max number of shots before overheating")]
    public float maxCharge;
    float currentCharge;
    [Tooltip("the delay after every shot before starting to cooldown")]
    public float cooldownDelay;
    [Tooltip("how fast the gun cooldowns itself")]
    public float cooldownRate;
    [Tooltip("how long is the overheating time before starting to cooldown again.")]
    public float overheatTime;
    public GameObject OverheatEffect;

    [SerializeField] HeatUI[] heatUIs;

    float defaultForce;
    float overheatTimer;
    float cooldownTimer;
    Player player;
    Vector2 lookDir;
    bool isOverheated;

    public override void Fire()
    {
        if (!player) return;
        Dash();
    }

    void Dash()
    {
        if (!canShoot) return;
        if (currentCharge <= 0)
        {
            overheatFeedbacks?.PlayFeedbacks(transform.position);
            return;
        }
        // if (!Input.GetButton("Fire1")) return;
        cooldownTimer = 0;
        currentCharge--;
        shootTimer = 0;
        canShoot = false;

        
        if (dashEffect)
        {
            dashEffectTimer = 0;
            dashEffect.SetActive(true);
        }
        //dashVFX?.Play();
        
        dashFeedbacks?.PlayFeedbacks(transform.position);
        player.isDashing = true;

        if (player.isConnected)
        {
            player.rb.AddForce(player.rb.velocity.normalized * dashForce);
            player.dashesWhileConnected++;
        }
        else
        {
            player.rb.AddForce(lookDir * dashForce);
        }

        StartCoroutine(StopDash());
    }

    IEnumerator StopDash()
    {
        yield return new WaitForSeconds(gunVariables.fireRate);
        player.isDashing = false;
    }
    
    void Cooldown()
    {
        if (currentCharge <= 0)
        {
            isOverheated = true;
            player.isOverheated = true;
            Overheat();
        }

        if (isOverheated) return;
        cooldownTimer += Time.deltaTime;
        if (!(cooldownTimer >= cooldownDelay)) return;
        
        if (currentCharge < maxCharge)
        {
            currentCharge += cooldownRate * Time.deltaTime;
        }
        else
        {
            currentCharge = maxCharge;
        }
    }

    void Overheat()
    {
        if (!isOverheated) return;
        overheatTimer += Time.deltaTime;
        overheatPulsVibration?.PlayFeedbacks(transform.position);
        if (overheatTimer < overheatTime) return;
        
        isOverheated = false;
        player.isOverheated = false;

        overheatTimer = 0;
        currentCharge += 1;
        cooldownTimer = cooldownDelay;
    }

    public override void Init()
    {
        player = GetComponent<Player>();
        currentCharge = maxCharge;
        defaultForce = dashForce;
        InputHandler.Instance.onDash += Fire;
    }

    void UpdateHeatUI()
    {
        for (int i = heatUIs.Length-1; i >= 0; i--)
        {
            if (currentCharge >= maxCharge)
            {
                heatUIs[i].gameObject.SetActive(false);
            }
            else
            {
                heatUIs[i].gameObject.SetActive(true);
                heatUIs[i].isHeated = isOverheated;

                if (Math.Abs(i - currentCharge) < .5f)
                {
                    heatUIs[(int)currentCharge].Activate();
                    heatUIs[(int)currentCharge].SetGrowing(true);
                    
                }else if (i < currentCharge)
                {
                    heatUIs[i].Activate();
                    heatUIs[i].ResetColor();
                    heatUIs[i].SetGrowing(false);
                }
                else
                {
                    heatUIs[i].deactive();
                }

                // heatUIs[i].enabled = i < maxCharge;
            }

        }
        
    }
    
    void RotateDashEffect()
    {
        if (!dashEffect.activeInHierarchy) return;
        
        Vector3 _moveDir = player.rb.velocity.normalized;
        float _moveAngle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg - 90f;
        dashEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _moveAngle));
    }

    public override void Run()
    {
        if (!player) return;

        base.Run();
        RotateDashEffect();
        UpdateHeatUI();
        dashForce = player.isInDanger ? isInDangerForce : defaultForce;
        
        lookDir = (gunVariables.muzzle.transform.position - transform.position).normalized;
        Cooldown();
        if(OverheatEffect) OverheatEffect.SetActive(isOverheated);
        
        if (!dashEffect) return;
        dashEffectTimer += Time.deltaTime;
        if (dashEffectTimer >= dashEffectDuration)
        {
            dashEffect.SetActive(false);
        }
    }

}
