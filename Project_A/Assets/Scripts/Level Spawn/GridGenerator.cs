using System;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public GameObject player;

    public float chunkWidth, chunkHeight;
    public float spawnTresh;

    readonly List<Vector3> fullIndexes = new List<Vector3>();
    Vector3 nextIndex;
    ChunkPool chunkPool;

    public void Init()
    {
        chunkPool = ChunkPool.Instance;
        Chunk _newChunk = chunkPool.GetFromPool();
        _newChunk.SetIndex(new Vector3(0,0,0));
        _newChunk.transform.position = CalcPosition();
        fullIndexes.Add(_newChunk.index);
        _newChunk.onChunkDestroy += OnChunkDestruct;
        Chunk.currentChunk = _newChunk;
    }

    public void Run()
    {
         CheckForSpawn();
    }

    void CheckForSpawn()
    {
        // if (Vector3.Distance(Chunk.currentChunk.transform.position, player.transform.position) < spawnTresh)
        //     return;

        SetNextIndex();
       // Debug.LogError("nextIndex: "+nextIndex);

     if (fullIndexes.FindIndex(a => a == nextIndex) != -1)
     {
       //  Debug.LogError("is full: "+ nextIndex);
         return;
     }

     Spawn();

        
    }

    void Spawn()
    {
        Chunk _newChunk = chunkPool.GetFromPool();
        _newChunk.transform.position = CalcPosition(); 
        _newChunk.SetIndex(nextIndex);
        fullIndexes.Add(_newChunk.index);
        _newChunk.onChunkDestroy += OnChunkDestruct;
    }

    float TurnTo360(float _angle)
    {
        if (_angle >= 0)
        {
            return _angle;
        }
        else
        {
            return _angle + 360;
        }
    }

    void SetNextIndex()
    {
        float angle = Vector3.SignedAngle(Vector3.right, player.transform.position - Chunk.currentChunk.transform.position, Vector3.forward);

        angle = TurnTo360(angle);
        // Debug.LogWarning(Chunk.currentChunk);
        // Debug.LogWarning(angle);


        if (angle >= 0 && angle <=22.5)
        {
            nextIndex = new Vector3(1,0,0);
        }

        if (angle > 22.5 && angle <= 67.5)
        {
            nextIndex = new Vector3(1,1,0);
        }

        if (angle > 67.5 && angle <=112.5)
        {
            nextIndex = new Vector3(0,1,0);
        }
        if (angle > 112.5 && angle <=157.5)
        {
            nextIndex = new Vector3(-1,1,0);
        }

        if (angle > 157.5 && angle <=202.5)
        {
            nextIndex = new Vector3(-1,0,0);
        }

        if (angle > 202.5 && angle <=247.5)
        {
            nextIndex = new Vector3(-1,-1,0);
        }
        if (angle > 247.5 && angle <= 292.5)
        {
            nextIndex = new Vector3(0,-1,0);
        }
        if (angle > 292.5 && angle <= 337.5)
        {
            nextIndex = new Vector3(1,-1,0);
        }
        if (angle > 337.5 && angle <=360)
        {
            nextIndex = new Vector3(1, 0,0);
        }

        nextIndex += Chunk.currentChunk.index;
    }


    void OnChunkDestruct(Vector3 _destroyedIndex)
    {
        int _listIndex = fullIndexes.FindIndex(a => a.x == _destroyedIndex.x && a.y == _destroyedIndex.y);
        if (_listIndex > -1)
        {
            Debug.Log("justDestroyed: "+ _destroyedIndex);
            fullIndexes.RemoveAt(_listIndex);
        }
    }

    Vector3 CalcPosition()
    {
        Vector3 _pos = new Vector3(nextIndex.x * chunkWidth, nextIndex.y * chunkHeight, 0);
        return _pos;
    }

}
