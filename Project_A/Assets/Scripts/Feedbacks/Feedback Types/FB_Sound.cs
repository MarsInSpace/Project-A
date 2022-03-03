using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class FB_Sound: Feedback
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] SoundSettings soundSettings;
    
    [Serializable]
    public class SoundSettings
    {
        [Range(0,1)]
        public float minVolume = 0.8f;
        [Range(0,2)]
        public float maxVolume = 2;
        [Range(0.5f,2)]
        public float minPitch = 0.88f;
        [Range(0.5f,2)]
        public float maxPitch = 1.2f;
        [Range(0,1)]
        public float spatialBlend = 1;

    }

    public override void PlayFeedback(Vector3 _position)
    {
        AudioManager.Instance.PlayOneShotSound("SFX", clips[Random.Range(0, clips.Length)], _position,
            Random.Range(soundSettings.minVolume, soundSettings.maxVolume), soundSettings.spatialBlend,
            Random.Range(soundSettings.minPitch, soundSettings.maxPitch));
    }
}
