using UnityEngine;
using Random = UnityEngine.Random;

public class LaunchAtStart : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float startSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartRandom();
    }
    
    void StartRandom()
    {
        Vector3 startDirection = new Vector3(Random.Range(-1000, 1000), Random.Range(-1000, 1000));
        rb.AddForce(startDirection.normalized * startSpeed);
    }
}
