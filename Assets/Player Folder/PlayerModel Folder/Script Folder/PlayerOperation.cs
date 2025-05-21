using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PlayerOperation : MonoBehaviour
{
    public Transform modelTransform; // ← モデル（見た目）だけを傾ける
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    float playerSpeed = 0f;
    float acceleration = 500f;
    float deceleration = 50f;
    float maxSpeed = 500f;
    float brakePower = 50f;

    float turnSpeed = 100f;

    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

    float yRotation = 0f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        HandleBankRotation();
        HandleWheelAnimation();
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.fixedDeltaTime;
        else
            playerSpeed -= deceleration * Time.fixedDeltaTime;

        if (Input.GetKey(KeyCode.S))
            playerSpeed -= brakePower * Time.fixedDeltaTime;

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        Vector3 moveDir = transform.forward;
        Vector3 groundNormal = Vector3.up;

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            groundNormal = hit.normal;
            moveDir = Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized;

            Quaternion targetRot = Quaternion.LookRotation(moveDir, groundNormal);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * 5f));
        }

        Vector3 newPos = rb.position + moveDir * playerSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void HandleBankRotation()
    {
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        yRotation += turn * turnSpeed * Time.deltaTime;

        // Zバンクだけモデルに適用
        targetBank = -turn * bankAngle;
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        if (modelTransform != null)
        {
            modelTransform.localRotation = Quaternion.Euler(0f, 0f, currentBank);
        }
    }

    void HandleWheelAnimation()
    {
        if (frontWheelRotator != null)
            frontWheelRotator.Rotate(playerSpeed);

        if (rearWheelRotator != null)
            rearWheelRotator.Rotate(playerSpeed);
    }
}

