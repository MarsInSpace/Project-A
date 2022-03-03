using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public abstract class Gun: MonoBehaviour
{
    protected bool canShoot;
    protected float shootTimer;

    [Serializable]
    public class Gun_Variables
    {
        public GameObject muzzle;
        public float fireRate;
        public float damage;
        public float projectileSpeed;
        public float fireKick;

        public Projectile projectilePrefab;
        [Space(10)]
        public AudioSource fireSound;
        public TextMeshProUGUI ammoTxt;
    }

    public Gun_Variables gunVariables;
    public abstract void Init();
    public virtual void Run()
    {
        if (canShoot) return;
        shootTimer += Time.deltaTime;
        if (shootTimer >= gunVariables.fireRate)
        {
            canShoot = true;
        }
    }
    


    public abstract void Fire();
}
