
using UnityEngine;

public class IndicatorCore : MonoBehaviour
{
    [SerializeField] HunterIndicator hunterIndicator;
    [HideInInspector]public float playerDistance;
    [SerializeField] private float screenBoundOffset = 0.9f;
    Camera cam;
    private Vector3 screenCentre;
    private Vector3 screenBounds;

    HunterBehaviour currentHunter;
    public Vector3 currentDistancePoint;
    public GameObject testObj;
    public void Setup(HunterBehaviour _currentHunter, Camera _cam)
    {
        currentHunter = _currentHunter;
        cam = _cam;
        screenCentre = new Vector3(Screen.width, Screen.height, 0) / 2;
        screenBounds = screenCentre * screenBoundOffset;

    }

    // Update is called once per frame
    void Update()
    {
        if (!currentHunter) return;
        RotateTowardsHunter();
        SetAlpha();
    }

    void SetAlpha()
    {
        hunterIndicator.SetAlpha(playerDistance);
    }

    void RotateTowardsHunter()
    {
        Vector3 _hunterPos = currentHunter.transform.position;
        Vector3 _hunterScreenPosition = GetScreenPosition(cam, currentDistancePoint);
        
        float angle = float.MinValue;
        GetIndicatorPositionAndAngle(ref _hunterScreenPosition, ref angle, screenCentre, screenBounds);
        transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg -90); // Sets the rotation for the arrow indicator.
     
        // Vector3 _direction = (_hunterScreenPosition - transform.position);
        // float _moveAngle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg -90;
        // transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _moveAngle));
    }
    
   
   Vector3 GetScreenPosition(Camera mainCamera, Vector3 targetPosition)
  {
      Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
      return screenPosition;
  }
   
   void GetIndicatorPositionAndAngle(ref Vector3 screenPosition, ref float angle, Vector3 screenCentre, Vector3 screenBounds)
    {
        // Our screenPosition's origin is screen's bottom-left corner.
        // But we have to get the arrow's screenPosition and rotation with respect to screenCentre.
        screenPosition -= screenCentre;

        // When the targets are behind the camera their projections on the screen (WorldToScreenPoint) are inverted,
        // so just invert them.
        if(screenPosition.z < 0)
        {
            screenPosition *= -1;
        }

        // Angle between the x-axis (bottom of screen) and a vector starting at zero(bottom-left corner of screen) and terminating at screenPosition.
        angle = Mathf.Atan2(screenPosition.y, screenPosition.x);
        // Slope of the line starting from zero and terminating at screenPosition.
        float slope = Mathf.Tan(angle);

        // Two point's line's form is (y2 - y1) = m (x2 - x1) + c, 
        // starting point (x1, y1) is screen botton-left (0, 0),
        // ending point (x2, y2) is one of the screenBounds,
        // m is the slope
        // c is y intercept which will be 0, as line is passing through origin.
        // Final equation will be y = mx.
        if(screenPosition.x > 0)
        {
            // Keep the x screen position to the maximum x bounds and
            // find the y screen position using y = mx.
            screenPosition = new Vector3(screenBounds.x, screenBounds.x * slope, 0);
        }
        else
        {
            screenPosition = new Vector3(-screenBounds.x, -screenBounds.x * slope, 0);
        }
        // Incase the y ScreenPosition exceeds the y screenBounds 
        if(screenPosition.y > screenBounds.y)
        {
            // Keep the y screen position to the maximum y bounds and
            // find the x screen position using x = y/m.
            screenPosition = new Vector3(screenBounds.y / slope, screenBounds.y, 0);
        }
        else if(screenPosition.y < -screenBounds.y)
        {
            screenPosition = new Vector3(-screenBounds.y / slope, -screenBounds.y, 0);
        }
        // Bring the ScreenPosition back to its original reference.
        screenPosition += screenCentre;
    }

}
