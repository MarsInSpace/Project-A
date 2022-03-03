using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputHandler : MonoBehaviour
{
    // Start is called before the first frame update
    public static InputHandler Instance;
    public Action onDash;
    public Action onPause;

    [HideInInspector]public Vector2 mouseInputVector;
    [HideInInspector]public Vector2 controllerInputVector;
    [HideInInspector]public bool isGrappling;
    [HideInInspector]public Gamepad currentGamepad;


    PlayerInput playerInput;
    PlayerActions playerActions;
    bool isGameplay;
    Camera cam;
    Ray ray;
    RaycastHit hit;
    
    void Awake()
    {
        Instance = this;
        playerInput = GetComponent<PlayerInput>();
        playerActions = new PlayerActions();
        cam = Camera.main;
        int _buildIndex = SceneManager.GetActiveScene().buildIndex;
        isGameplay = _buildIndex != 0;
    }

    void Update()
    {
        currentGamepad = GetGamepad();
        if (isGameplay)
        {
            isGrappling = playerActions.PlayerControls.Grapple.ReadValue<float>() > 0f;
        
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, float.MaxValue, LayerMask.GetMask("MousePlane")))
            {
                mouseInputVector = hit.point;
            }

            controllerInputVector = playerActions.PlayerControls.ControllerAim.ReadValue<Vector2>();
            playerInput.SwitchCurrentActionMap(GameManager.isPaused ? "UI" : "PlayerControls");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
      
    }


    private Gamepad GetGamepad()
    {
        return Gamepad.all.FirstOrDefault(g => playerInput.devices.Any(d => d.deviceId == g.deviceId));
    }

    void OnDashListener(InputAction.CallbackContext _context)
    {
        if (_context.performed && !GameManager.isPaused)
        {
            onDash?.Invoke();
        }
    }


    void OnPauseListener(InputAction.CallbackContext _context)
    {
        if (_context.performed)
        {
            onPause?.Invoke();
        }
    }

    public void UnregisterEvents()
    {
        playerActions.PlayerControls.Disable();
        playerActions.PlayerControls.Dash.performed -= OnDashListener;
        playerActions.PlayerControls.Pause.performed -= OnPauseListener;
    }
    
    public void RegisterEvents()
    {
        playerActions.PlayerControls.Enable();
        playerActions.PlayerControls.Dash.performed += OnDashListener;
        playerActions.PlayerControls.Pause.performed += OnPauseListener;
    }
}
