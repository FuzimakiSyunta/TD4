using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    public Transform modelTransform; // モデル（見た目）だけを傾ける
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    float playerSpeed = 0f;
    float acceleration = 500f;
    float deceleration = 50f;
    float maxSpeed = 500f;
    float brakePower = 50f;

    float turnSpeed = 100f;
    float rotationY = 0f;

    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

  
    void Start()
    {
       // stunt = GetComponent<Stunt>();

    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        HandleBankRotation();
        HandleWheelAnimation();
    }

    void HandleInput()
    {
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        rotationY += turn * turnSpeed * Time.deltaTime;

        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else
            playerSpeed -= deceleration * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            playerSpeed -= brakePower * Time.deltaTime;

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        //アニメーションの処理
       // stunt.PlayerAnimation();
    }

    void HandleMovement()
    {
        // 地形に沿った移動方向
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        Vector3 moveDir = transform.forward;
        Vector3 groundNormal = Vector3.up;

        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            groundNormal = hit.normal;
            moveDir = Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized;

            Quaternion targetRot = Quaternion.LookRotation(moveDir, groundNormal);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }

        transform.position += moveDir * playerSpeed * Time.deltaTime;
    }

    void HandleBankRotation()
    {
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

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