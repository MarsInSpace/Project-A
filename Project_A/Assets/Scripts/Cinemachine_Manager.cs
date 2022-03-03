using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Experimental.Rendering.LWRP;

public class Cinemachine_Manager : MonoBehaviour
{
    public static Cinemachine_Manager Instance; /*{ get; private set; }*/
    CinemachineVirtualCamera cVirtualCam;
    CinemachineBasicMultiChannelPerlin perlin;
    CinemachineTransposer transposer;
    Volume volume;
    HunterBehaviour hunter;
    
    Vector3 defaultFollowOffset;
    public float minZoom;
    public float maxZoom = -80f;
    public float currentMaxZoom;
    [HideInInspector]public float defaultZoom;

    float shakeTimer;
    Player player;
    void Awake()
    {
        volume = FindObjectOfType<Volume>();
        hunter = FindObjectOfType<HunterBehaviour>();
        defaultZoom = maxZoom;
        Instance = this;
        cVirtualCam = GetComponent<CinemachineVirtualCamera>();
        perlin = cVirtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        transposer = cVirtualCam.GetCinemachineComponent<CinemachineTransposer>();
        defaultFollowOffset = new Vector3(0, 2, -20);
        player = FindObjectOfType<Player>();
    }

    private void LateUpdate()
    {
        if (Player.currentSpeed > 20)
        {
              transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, currentMaxZoom), .2f * Time.deltaTime);
   
        }
        else if (Player.currentSpeed < 10)
        {
            transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, new Vector3(transposer.m_FollowOffset.x, transposer.m_FollowOffset.y, minZoom), .2f * Time.deltaTime) ;
        }
        else
        {
            transposer.m_FollowOffset.z = Mathf.Lerp(transposer.m_FollowOffset.z, -20, .5f * Time.deltaTime) ;
        }
       

    }

 
    void Update()
    {
        shakeTimer -= Time.deltaTime;
        if (shakeTimer <= 0)
        {
            perlin.m_AmplitudeGain = 0;
        }

        if (currentMaxZoom < maxZoom)
        {
            currentMaxZoom = maxZoom;
        }

    }

    public void CameraShake(float _intensity, float _time)
    {
        perlin.m_AmplitudeGain = _intensity;
        shakeTimer = _time;
    }
}
