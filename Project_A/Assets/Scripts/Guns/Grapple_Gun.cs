using UnityEngine;
using System;

public class Grapple_Gun : Gun
{
   //private Variables:

    public Vector3 grapplePoint;
    Player player;
    SpringJoint springJoint;
    GameObject connectedGameObject;
    Platform platformInRange;
    Platform connectedPlatform;

    bool canGrapple;    
    bool isConnected;
    float currentDist;
    float minHookSpeed;
    RaycastHit hit;
    readonly RaycastHit[] laserHits = new RaycastHit[20];

    [Header("Hook Attributes")]
    [SerializeField] LayerMask whatIsGrappable;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] GameObject pointer;
    [Range(1,30)]
    [SerializeField] float speedMultiplier = 2;
    [SerializeField] bool hasLaser;
    [SerializeField] AudioSource swingAudio;

    bool isLaser;

    [Range(11, 100)]
    [SerializeField] float MaxHookRange;
    [Range(0, 10)]
    [SerializeField] float minHookRange;

    
    [Tooltip("when it is not instant pull then it is a spring Joint:")]
    [SerializeField] springJointVariables jointVariables;

    [Serializable]
    public class springJointVariables
    {
        public float pushBackForce = 150;
        public float maxDistMultiplier = .8f;
        public float minDistMultiplier = .25f;
        public float spring = 4.5f;
        public float damper = 7;
        public float massScale = 4.5f;
    }
   


    public override void Fire()
    {
        if (InputHandler.Instance.isGrappling)
        {
            StartGrapple();
        }
        else
        {
            StopFiring();
        }
     
    }
    
    public override void Init()
    {
        lineRenderer.enabled = false;
        player = GetComponent<Player>();
      
    }

   public override void Run()
    {

        currentDist = Vector3.Distance(transform.position, hit.point);

        CheckGrapple();
        Fire();
        
        if (isConnected)
        {
            player.isConnected = true;
            Cinemachine_Manager.Instance.currentMaxZoom = -60;
            currentDist = Vector3.Distance(transform.position, grapplePoint);

            LaserDetection();

            if (Player.currentSpeed < minHookSpeed)
            {
                player.rb.velocity = player.rb.velocity.normalized * minHookSpeed;
            }

            var _velocity = player.rb.velocity;
            _velocity += _velocity.normalized * (speedMultiplier * Time.deltaTime);
            player.rb.velocity = _velocity;

            CheckForStopping();

        }
        else
        {
            Cinemachine_Manager.Instance.currentMaxZoom = Cinemachine_Manager.Instance.defaultZoom;
            player.isConnected = false;

        }
    }

    void CheckForStopping()
    {
        if (player.dashesWhileConnected >= 3)
        {
            connectedPlatform?.GetDestroyed(transform.position);
            StopFiring();
        }
        if (!hit.transform)
        {
            StopFiring();
        }

        if (connectedPlatform)
        {
            if (connectedPlatform.isRevivng)
            {
                StopFiring();
            }
        }

        if (!player.isInBooster) return;
        canGrapple = false;
        StopFiring();

    }

    void LaserDetection()
    {
        if (!hasLaser) return;
        
        isLaser = Player.currentSpeed >= player.unstoppableSpeed;
        if (isLaser && isConnected)
        {
            lineRenderer.material = player.unstoppableTrailMat;
            Vector3 _dir = (lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0)).normalized;
            var _hits = Physics.RaycastNonAlloc(lineRenderer.GetPosition(0), _dir, laserHits, currentDist, whatIsGrappable);
            if (laserHits.Length <= 0) return;
            for (int i = 0; i < _hits; i++)
            {
                if (laserHits[i].transform.gameObject == null) continue;
                if (connectedGameObject == null) continue;
                if (laserHits[i].transform.gameObject == connectedGameObject) continue;

                Platform _p = laserHits[i].transform.gameObject.GetComponent<Platform>();
                if (_p != null)
                {
                    _p.GetDestroyed(laserHits[i].point);
                }
            }
        }
        else
        {
            lineRenderer.material = player.normalTrailMat;
        }


    }
   

    void CheckGrapple()
    {
        if (!isConnected && !player.isInBooster)
        {
            if (Physics.Raycast(gunVariables.muzzle.transform.position, gunVariables.muzzle.transform.up, out hit, MaxHookRange, whatIsGrappable))
            {
                if (currentDist >= minHookRange)
                {
                    pointer.SetActive(true);
                    pointer.transform.position = hit.point;
                    canGrapple = true;
                }
                else
                {
                    pointer.SetActive(false);
                    canGrapple = false;
                }
            }
            else
            {
                pointer.SetActive(false);
                canGrapple = false;
            }
        }        
        else
        {
            pointer.transform.position = hit.point;        
        }
    }

 
    void StartGrapple()
    {
        if (!canGrapple) return;
        grapplePoint = hit.point;

        if (hit.transform != null)
        {
            if (connectedGameObject == null)
            {
                connectedGameObject = hit.transform.gameObject;
                connectedPlatform = connectedGameObject.GetComponent<Platform>();
                if (connectedPlatform) connectedPlatform.isConnected = true;
            }
        }           

        if (connectedGameObject == null) return;
        if (currentDist < minHookRange) return;
        ConnectHook(grapplePoint);
    }

    void ConnectHook(Vector3 _grapplePoint)
    {
        minHookSpeed = Player.currentSpeed;
        if (!springJoint)
        {
            springJoint = gameObject.AddComponent<SpringJoint>();
        }  
        
        springJoint.autoConfigureConnectedAnchor = false;
        springJoint.connectedAnchor = _grapplePoint;

        float _dist = Vector3.Distance(transform.position, _grapplePoint);
        springJoint.maxDistance = _dist * jointVariables.maxDistMultiplier;
        springJoint.minDistance = _dist * jointVariables.minDistMultiplier;

        springJoint.spring = jointVariables.spring;
        springJoint.damper = jointVariables.damper;
        springJoint.massScale = jointVariables.massScale;                      
        
      
        lineRenderer.positionCount = 2;
        isConnected = true;
    }

 
    void DrawRope()
    {
        if (!springJoint) return;
        if (!isConnected) return;
        
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, gunVariables.muzzle.transform.position);
        lineRenderer.SetPosition(1, grapplePoint);
    }


    private void LateUpdate()
    {
        DrawRope();
    }

    void StopFiring()
    {
        lineRenderer.enabled = false;
        connectedGameObject = null;
        if(connectedPlatform) connectedPlatform.isConnected = false;
        connectedPlatform = null;
        player.dashesWhileConnected = 0;
        
        lineRenderer.positionCount = 0; 
        Destroy(springJoint);         
        isConnected = false;
        isLaser = false;
    }

   
}
