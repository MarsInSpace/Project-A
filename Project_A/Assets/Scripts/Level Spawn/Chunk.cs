using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public event dlg_ChunkIndex onChunkDestroy;
    [SerializeField] float destroyRange;
    public static Chunk currentChunk;
    Platform[] platforms;
    Booster[] boosters;
    Player player;
    internal Vector3 index;

    public void Init()
    {
        player = FindObjectOfType<Player>();
        boosters = GetComponentsInChildren<Booster>();
        platforms = GetComponentsInChildren<Platform>();

        foreach (Platform _p in platforms)
        {
            _p.Init();
            _p.player = player;
        }

        foreach (Booster _b in boosters)
        {
            _b.player = player;
        }
    }
    public void SetIndex(Vector3 _index)
    {
        index = _index;
       // StartCoroutine(CheckDistanceDelay());
    }

    void Update()
    {
        if(!player) return;
        if (Vector3.Distance(transform.position,player.transform.position) > destroyRange)
        {
            SelfDestruct();
        }
    }

    void SelfDestruct()
    {
        onChunkDestroy?.Invoke(index);
        // Debug.Log("request to Destroy: "+gameObject);
        ChunkPool.Instance.AddToPool(this);
    }

    // IEnumerator CheckDistanceDelay()
    // {
    //     yield return new WaitForSeconds(4);
    //     CheckDist();
    // }
    //
    // void CheckDist()
    // {
    //     if (Vector3.Distance(transform.position,player.transform.position) > destroyRange)
    //     {
    //         SelfDestruct();
    //     }
    //     else
    //     {
    //         StartCoroutine(CheckDistanceDelay());
    //     }
    //
    // }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
          currentChunk = this;
    }
}
