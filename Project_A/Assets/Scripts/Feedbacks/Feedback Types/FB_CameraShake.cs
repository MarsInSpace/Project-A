using UnityEngine;

    public class FB_CameraShake: Feedback
    {
        [SerializeField] [Range(0,3)]float shakeDuration = .3f;
        [SerializeField] [Range(0,10)]float shakeIntensity = 3f;
        public override void PlayFeedback(Vector3 _position)
        {
            Cinemachine_Manager.Instance.CameraShake(shakeIntensity,shakeDuration);
        }
    }
