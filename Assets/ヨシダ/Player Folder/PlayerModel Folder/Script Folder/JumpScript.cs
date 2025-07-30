using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private Rigidbody rb;
    public float rotationX = 0f;
    private float turnX;
    float jumpForce = 2f;
    float floatDuration = 0.6f;
    float floatDrag = 2f;
    bool isJumping = false;
    float jumpCooldown = 0.3f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rotationX += turnX * 50f * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -30f, 30f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {
            rotationX = -18f;
            //Debug.Log("ç‚Ç≈Ç∑");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jump") && !isJumping)
        {
            Vector3 jumpDirection = (Vector3.up * 1.2f + transform.forward * 0.5f + transform.right * 0.3f).normalized;

            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);

          //  Debug.Log("Jump Direction: " + jumpDirection);

            StartCoroutine(ReduceGravityTemporarily());
            StartCoroutine(TemporaryJumpLock());
        }
    }

    private IEnumerator ReduceGravityTemporarily()
    {
        float originalDrag = rb.drag;
        rb.drag = floatDrag;

        yield return new WaitForSeconds(floatDuration);

        rb.drag = originalDrag;
    }

    IEnumerator TemporaryJumpLock()
    {
        isJumping = true;
        yield return new WaitForSeconds(jumpCooldown);
        isJumping = false;
    }
}
