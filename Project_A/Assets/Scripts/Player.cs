using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    public event dlg_VoidNoArg onDie;
    //private stat variables
    public Rigidbody rb;
    Health health;
    public Material overheatMaterial;
    public static float currentSpeed;
    [Range(10,500)]
    public float maxSpeed;
    [Range(10, 500)]
    public float unstoppableSpeed;
    
   
    [SerializeField] TrailRenderer unstoppableTrail;
    public Material unstoppableTrailMat;
    public Material normalTrailMat;
    
    public bool isInBooster;
    public bool isConnected;
    public bool isOverheated;
    public bool isDashing;
    public bool isInDanger;
    bool checkDangerAhead;
    public int dashesWhileConnected;
    public GameObject unstopableEffect;
    [SerializeField] LayerMask deadlyPlatform;
    [SerializeField] Feedbacks dangerFeedbacks;
    [SerializeField] Feedbacks nearMissFeedbacks;
    bool isNearMissActive;

    [Header("Weapon")]
    [Tooltip("used for rotating towards the look position")]
    [SerializeField] GameObject aimObject;
 
    public List<Gun> guns = new List<Gun>();
   
    [HideInInspector] public Vector2 controllerVector2;
    [HideInInspector] public Vector2 mouseVector2;


    MeshRenderer meshRenderer;
    Material defaultMat;
    InputHandler inputHandler;
    void Awake()
    {
        health = GetComponent<Health>();
        health.onDie += OnDieListener;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        defaultMat = meshRenderer.material;
    }

    public void Init()
    {
        InitGuns();
        inputHandler = InputHandler.Instance;
        isNearMissActive = true;
    }
    
    void InitGuns()
    {
        foreach (Gun _g in guns)
        {
            _g.Init();
        }      
    }
    public void FixedRun()
    {
        HandleAim();
        CheckSpeed();
    }

    void HandleAim()
    {
        controllerVector2 = inputHandler.controllerInputVector;
        mouseVector2 = inputHandler.mouseInputVector  - (Vector2)transform.position;
        float _angle;
        if (controllerVector2.magnitude > .1f)
        {
            _angle = Mathf.Atan2(controllerVector2.y, controllerVector2.x) * Mathf.Rad2Deg - 90f;
            aimObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _angle));
        }
        else if(inputHandler.currentGamepad == null)
        {
             _angle = Mathf.Atan2(mouseVector2.y, mouseVector2.x) * Mathf.Rad2Deg - 90f;
             aimObject.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _angle));
        }
    }
    public void Run()
    {
        foreach (Gun _g in guns)
        {
            _g.Run();
        }

        RotateMeteor();
        OverheatMaterial();
        IsInDanger();
    }

    void OverheatMaterial()
    {
        if (isOverheated)
        {
            meshRenderer.material = overheatMaterial;
            unstoppableTrail.enabled = false;
        }
        else
        {
            meshRenderer.material = defaultMat;
            unstoppableTrail.enabled = true;
        }
    }
    void RotateMeteor()
    {
        unstopableEffect.SetActive(currentSpeed >= unstoppableSpeed);
        if (!unstopableEffect.activeInHierarchy) return;
        
        Vector3 _moveDir = rb.velocity.normalized;
        float _moveAngle = Mathf.Atan2(_moveDir.y, _moveDir.x) * Mathf.Rad2Deg - 90f;
        unstopableEffect.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _moveAngle));
    }
    void CheckSpeed()
    {
        currentSpeed = rb.velocity.magnitude;

        if (!(currentSpeed > maxSpeed * 1.5f) || isDashing) return;
        Vector3 _velocity = rb.velocity;
        rb.velocity = Vector3.Lerp(_velocity, _velocity.normalized * maxSpeed, .3f  * Time.fixedDeltaTime);
        
    }

    void OnDieListener()
    {
        onDie?.Invoke();
    }

    void IsInDanger()
    {
        if (!checkDangerAhead) return;
        RaycastHit _hit;
        isInDanger = Physics.Raycast(rb.position, rb.velocity.normalized, out _hit, 25, deadlyPlatform);
        if(isInDanger)
            dangerFeedbacks.PlayFeedbacks(transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("DangerZone")) return;
        checkDangerAhead = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DangerZone"))
        {
            checkDangerAhead = false;
        }

        if (!other.CompareTag("NearMiss")) return;
        if (!isNearMissActive) return;
        isNearMissActive = false;
        ScoreManager.Instance.AddScore(500);
        nearMissFeedbacks?.PlayFeedbacks(transform.position);
        StartCoroutine(resetNearMiss());
    }

    IEnumerator resetNearMiss()
    {
        yield return new WaitForSeconds(3);
        isNearMissActive = true;
    }
}
