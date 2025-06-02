using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    public float gravity = 9.81f;
    private float verticalVelocity = 0f;
    public Transform frontWheel;
    public float rayLength = 0.2f;
    public LayerMask groundLayer;
    void Update()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        transform.position += Vector3.up * verticalVelocity * Time.deltaTime;

    }

    bool IsWheelTouchingGround()
    {
        return Physics.Raycast(frontWheel.position, Vector3.down, rayLength, groundLayer);
    }


}

