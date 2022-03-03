using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ScoreBreakablePlatform : Platform
{
    [Serializable]
    class ExtraScoreDetails
    {
        public float chance = 60;
        public int extraScore = 100;
        public float duration = 4;
        public Material extraScoreMaterial;
        public global::Feedbacks extraScoreFeedbacks;
    } 
   
   [SerializeField] ExtraScoreDetails extraScoreDetails;
   int defaultScore;
   global::Feedbacks defaultFeedbacks;
   Material defaultMaterial;
   void Start()
   {
       defaultScore = destroyScore;
       defaultMaterial = defaultMat;
       defaultFeedbacks = feedbacks.explosionFeedbacks;
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

}
