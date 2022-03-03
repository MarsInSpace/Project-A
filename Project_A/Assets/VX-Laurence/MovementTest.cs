using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MovementTest : MonoBehaviour
{
    public float speed = 5.0f;
    public Rigidbody player;
    public VisualEffect dashBoostEffect;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        player.position += new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
        player.velocity = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;


        if(Input.GetKeyDown(KeyCode.P))
        {
            dashBoostEffect.Play();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            dashBoostEffect.Stop();
        }
    }

}
