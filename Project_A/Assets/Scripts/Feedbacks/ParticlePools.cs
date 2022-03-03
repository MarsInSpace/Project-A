using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParticlePools : MonoBehaviour
{
    [Serializable]
    public class Pool
    {
        public ParticleType particleType;
        public GameObject particlePrefab;
        public int poolSize;
    }
    
    [SerializeField] List<Pool> pools = new List<Pool>();
    Dictionary<string, Queue<GameObject>> poolDictionary;

    public static ParticlePools Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        GrowPool();
    }

    void GrowPool()
    {
        foreach (Pool _pool in pools)
        {
            Queue<GameObject> _prefabPool = new Queue<GameObject>();
            for (int i = 0; i < _pool.poolSize; i++)
            {
                GameObject _obj = Instantiate(_pool.particlePrefab);
                _obj.SetActive(false);
                _prefabPool.Enqueue(_obj);
            }
            poolDictionary.Add(_pool.particleType.ToString(), _prefabPool);
        }
    }

    public void AddToPool(string _Tag, GameObject _obj)
    {
        _obj.gameObject.SetActive(false);
        poolDictionary[_Tag].Enqueue(_obj);
    }

    public GameObject GetFromPool(string _tag, Vector3 _position)
    {
        if (!poolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("pool with tag "+ _tag + " doesn't exist");
            return null;
        }
    
        GameObject _objToSpawn = poolDictionary[_tag].Dequeue();
        
        _objToSpawn.SetActive(true);
        _objToSpawn.transform.position = _position;
        
      //  poolDictionary[_tag].Enqueue(_objToSpawn);

        return _objToSpawn;
    }

}

public enum ParticleType
{
    Explosion,
    PlayerDeath,
    ExplosionExtra
}