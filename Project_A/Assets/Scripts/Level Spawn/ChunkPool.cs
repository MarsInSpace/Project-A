using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkPool : MonoBehaviour
{
    [SerializeField]
    Chunk[] levelPrefabs;
    [SerializeField] int poolAmount;
    readonly Queue<Chunk> availableObjs = new Queue<Chunk>();

    public static ChunkPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    void GrowPool()
    {
        for (int i = 0; i < poolAmount; i++)
        {
            if (i > levelPrefabs.Length - 1)
            {
                int x = Random.Range(0, levelPrefabs.Length);
                var _intanceToAdd = Instantiate(levelPrefabs[x]);
               // _intanceToAdd.transform.SetParent(transform);
                _intanceToAdd.Init();
                AddToPool(_intanceToAdd);
            }
            else
            {
                var _intanceToAdd = Instantiate(levelPrefabs[i]);
              //  _intanceToAdd.transform.SetParent(transform);
                _intanceToAdd.Init();

                AddToPool(_intanceToAdd);
            }
        }
    }

    public void AddToPool(Chunk _level)
    {
        _level.gameObject.SetActive(false);
        availableObjs.Enqueue(_level);
    }

    public Chunk GetFromPool()
    {
        if (availableObjs.Count == 0)
        {
            Debug.Log("growing" + availableObjs.Count.ToString());
            GrowPool();
        }

        Chunk _instance = availableObjs.Dequeue();

        _instance.gameObject.SetActive(true);
        return _instance;
    }

}
