using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;


public class Platform : MonoBehaviour
{
   
    public bool isBreakable = true;

    public int destroyScore = 200;
    
    [Serializable]
    public class Feedbacks
    {
        public global::Feedbacks explosionFeedbacks;
        public global::Feedbacks bounceFeedbacks;
        public global::Feedbacks scoreFeedback;
    }
    [Serializable]
     class Materials
     {
         public Material connectedMaterial;
         public Material firstDashMat;
         public Material secondDashMat;
     }
    [Serializable]
    class ExtraScoreDetails
    {
        public float chance = 60;
        public int extraScore = 100;
        public float duration = 4;
        public Material extraScoreMaterial;
        public global::Feedbacks extraScoreFeedbacks;
    } 

    [Tooltip("Add Feedbacks Class to an empty child of the Platform and add as many feedbacks as you like to each of them")]
    public Feedbacks feedbacks;
    [Space(5)]
    [SerializeField] Collider theCollider;
    [SerializeField] Materials materials;
    
    
    [HideInInspector] public Player player;
    [HideInInspector]public bool isConnected;
    [HideInInspector] public bool isRevivng;
    
    [Space(5)]
    [SerializeField] float reviveDelay;
    float reviveTimer;
    
    MeshRenderer mr;
    protected Material defaultMat;
 
   
    [SerializeField] ExtraScoreDetails extraScoreDetails;
    int defaultScore;
    global::Feedbacks defaultFeedbacks;
    Material defaultMaterial;
    void Start()
    {
        defaultScore = destroyScore;
        defaultMaterial = defaultMat;
        defaultFeedbacks = feedbacks.explosionFeedbacks;
        if (extraScoreDetails.chance == 0) return;
        StartCoroutine(CheckIfScore());
    }

    IEnumerator CheckIfScore()
    {
     
        int _random = Random.Range(1, 5);
        yield return new WaitForSeconds(_random);

        float _randomChance = Random.Range(0, 100);
        if (_randomChance <= extraScoreDetails.chance)
        {
            destroyScore += extraScoreDetails.extraScore;
            if (extraScoreDetails.extraScoreMaterial)
            {
                defaultMat = extraScoreDetails.extraScoreMaterial;
            }
            if (extraScoreDetails.extraScoreFeedbacks)
            {
                feedbacks.explosionFeedbacks = extraScoreDetails.extraScoreFeedbacks;
            }
            StartCoroutine(ReturnToNormal());
        }
    }

    IEnumerator ReturnToNormal()
    {
        yield return new WaitForSeconds(extraScoreDetails.duration);
       
        destroyScore = defaultScore;
        SetupMaterial(extraScoreDetails.extraScoreMaterial);
        feedbacks.explosionFeedbacks = defaultFeedbacks;
        defaultMat = defaultMaterial;
        StartCoroutine(CheckIfScore());
    }


    public void Init()
    {
        mr = GetComponentInChildren<MeshRenderer>();
        defaultMat = mr.material;
    }
    private void Update()
    {
        if (!player) return;
        if (isRevivng)
        {
            reviveTimer += Time.deltaTime;
            if (reviveTimer >= reviveDelay)
            {
                mr.enabled = true;
                reviveTimer = 0;
                isRevivng = false;
            }
        }
        else
        {
            CheckPlayerSpeed();
            SetupMaterial(defaultMat);
        }
    }

  
    void CheckPlayerSpeed()
    {
        if (!isBreakable) return;
        
        if (Player.currentSpeed >= player.unstoppableSpeed)
        {
            theCollider.isTrigger = true;
        }
        else if (!isRevivng)
        {
            theCollider.isTrigger = false;
        }
    }

    public void SetupMaterial(Material mat = null)
    {
        if (isConnected)
        {
            switch (player.dashesWhileConnected)
            {
                case 0: mr.material = materials.connectedMaterial;
                    break;
                case 1: mr.material = materials.firstDashMat;
                    break;
                case 2: mr.material = materials.secondDashMat;
                    break;
                default:
                    break;
            }
        }
        else if (mat)
            mr.material = mat;
    }
  

    public void GetDestroyed(Vector3 _hitPoint)
    {
        if (!isRevivng)
        {
            Vector3 _pos = transform.position;
            feedbacks.explosionFeedbacks?.PlayFeedbacks(_pos);
            feedbacks.scoreFeedback?.PlayFeedbacks(_pos);
            
            ScoreManager.Instance.AddMultiplier();
            ScoreManager.Instance.AddScore(destroyScore);
        }
   
        mr.enabled = false;
        reviveTimer = 0;
        isRevivng = true;
    }

    private void OnTriggerEnter(Collider _hitInfo)
    {
        if (_hitInfo.gameObject.CompareTag("Player"))
        {
            if(isBreakable)
                GetDestroyed(_hitInfo.transform.position);         
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           feedbacks.bounceFeedbacks?.PlayFeedbacks(transform.position);
        }    
    }
}
