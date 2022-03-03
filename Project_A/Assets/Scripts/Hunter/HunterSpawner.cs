using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterSpawner : MonoBehaviour
{
    
    [SerializeField] Player player;
    Collider hunterCollider;

    [SerializeField] int triggerScore;
    [SerializeField] float spawnInterval,intervalReduce,minInterval, respawnRange,minRespawnRange, SpeedIncreasePerWave, distanceReducePerWave;

    [SerializeField] HunterBehaviour hunterPrefab;
    [SerializeField] IndicatorCore hunterUICore;
    [Header("Hunter Sound")]
    [SerializeField] AudioSource hunterAudio;
    [SerializeField] float maxVol = .45f;
    [SerializeField] float minVol = 0.04f;


    
    private Camera mainCamera;
    HunterBehaviour hunter;
    Transform playerTransform;
    Rigidbody playerRigidbody;

    float playerDistance;
    bool isHunterDeployed = false;

    void Awake()
    {
        mainCamera = Camera.main;
        playerTransform = player.transform;
        playerRigidbody = player.rb;
        hunter = Instantiate(hunterPrefab);
        hunterCollider = hunter.impactCollider;
        hunter.gameObject.SetActive(false);
        hunterUICore.Setup(hunter, mainCamera);
    }
    
    public void Run()
    {
        if (ScoreManager.Instance.currentScore >= triggerScore && !isHunterDeployed)
        {
            DeployHunter();
            isHunterDeployed = true;
        }
        
        if (!hunter.gameObject.activeInHierarchy) return;
        CalcDistance();
        hunterUICore.playerDistance = playerDistance;
        CalcAudioVolume();
    }

    void DeployHunter(){
        hunter.gameObject.SetActive(true);
        hunterUICore.gameObject.SetActive(true);
        hunterAudio.Play();
        
        hunter.Init(player.transform, playerRigidbody,SpeedIncreasePerWave, distanceReducePerWave);
        hunter.LookAtTarget();
        respawnRange -= 100;
        if (respawnRange <= minRespawnRange) respawnRange = minRespawnRange;
        StartCoroutine(CheckForSpawn());
    }

    void CalcDistance()
    {
        hunterUICore.currentDistancePoint = hunterCollider.ClosestPoint(playerTransform.position);
        playerDistance = (hunterUICore.currentDistancePoint - playerTransform.position).magnitude;
    }
    
     void CalcAudioVolume()
    {
        if (!hunterAudio.clip || !hunterAudio) return;
      
        var _currentRange = (playerDistance / 1000);
        var _newVolume = 1 - _currentRange;
        if (_newVolume < minVol)
        {
            _newVolume = minVol;
        }

        if (_newVolume >= maxVol)
        {
            _newVolume = maxVol;
        }

        hunterAudio.volume = Mathf.Lerp( hunterAudio.volume, _newVolume, .8f * 5 * Time.deltaTime);
    }
    void DespawnHunter()
    {
        hunter.gameObject.SetActive(false);
        hunterUICore.gameObject.SetActive(false);
        hunterAudio.Pause();
        

        spawnInterval -= intervalReduce;
        if (spawnInterval < minInterval)
        {
            spawnInterval = minInterval;
        }
        StartCoroutine(DeployAfterInterval());
    }
    IEnumerator DeployAfterInterval()
    {
        yield return new WaitForSeconds(spawnInterval);
        DeployHunter();
    }

    IEnumerator CheckForSpawn()
    {
        yield return new WaitForSeconds(5);
        if (playerDistance < respawnRange)
            StartCoroutine(CheckForSpawn());
        else
            DespawnHunter();
    }

}
