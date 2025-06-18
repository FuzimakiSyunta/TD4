using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;

    public Transform modelTransform;
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    float playerSpeed = 0f;
    float acceleration = 35f;
    float deceleration = 50f;
    float maxSpeed = 600f;
    float brakePower = 50f;

    float turnSpeed = 100f;
    float rotationY = 0f;

    float bankAngle = 10f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

    GoalScript goalScript;

    bool wasGrounded = true;

    void Start()
    {
        goalScript = GameObject.Find("bike body 1").GetComponent<GoalScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2538f, 1369f);
        pos.z = Mathf.Clamp(pos.z, -3270f, 3663f);
        transform.position = pos;

        if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        {
            HandleInput();
            HandleMovement();
            HandleBankRotation();
            HandleWheelAnimation();
        }
    }

    void HandleInput()
    {
        float turn = 0f;

        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) turn = -1f;
            else if (Input.GetKey(KeyCode.D)) turn = 1f;
        }

        rotationY += turn * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            playerSpeed -= acceleration * Time.deltaTime;
        else
            playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);

        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);
    }

    void HandleMovement()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        Vector3 moveDir = transform.forward;
        Vector3 groundNormal = Vector3.up;

        bool isGrounded = false;

        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            isGrounded = true;

            groundNormal = hit.normal;
            float slopeAngle = Vector3.Angle(Vector3.up, groundNormal);
            float slopeLimit = 50f;

            if (slopeAngle <= slopeLimit)
            {
                moveDir = Vector3.ProjectOnPlane(transform.forward, groundNormal).normalized;

                Quaternion targetRot = Quaternion.LookRotation(moveDir, groundNormal);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 3f);

                Vector3 targetPos = new Vector3(transform.position.x, hit.point.y, transform.position.z);
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);

                float slopeFactor = Vector3.Dot(groundNormal, moveDir);
                float slopeEffect = 1f - Mathf.Clamp01(-slopeFactor);
                playerSpeed *= Mathf.Lerp(1f, 0.95f, 1f - slopeEffect);
            }
            else
            {
                playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, Time.deltaTime * 50f);
            }
        }

        // 坂の終わり（地面がなくなった）瞬間にジャンプ
        if (!isGrounded && wasGrounded)
        {
            Jump();
        }

        wasGrounded = isGrounded;

        Vector3 checkDir = playerSpeed >= 0 ? moveDir : -moveDir;
        float checkDistance = Mathf.Abs(playerSpeed) * Time.deltaTime + 0.1f;

        if (Physics.Raycast(transform.position, checkDir, out RaycastHit wallHit, checkDistance))
        {
            if (wallHit.collider.CompareTag("Wall"))
            {
                playerSpeed = 0f;
                return;
            }
        }

        transform.position += moveDir * playerSpeed * Time.deltaTime;
    }

    void Jump()
    {
        // 上方向に3ユニットジャンプ（演出に合わせて調整可能）
        transform.position += Vector3.up * 3f;
        Debug.Log("ジャンプ！");
    }

    void HandleBankRotation()
    {
        float turn = 0f;

        if (Mathf.Abs(playerSpeed) > 5f)
        {
            if (Input.GetKey(KeyCode.A)) turn = -1f;
            else if (Input.GetKey(KeyCode.D)) turn = 1f;
        }

        targetBank = -turn * bankAngle;
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        if (modelTransform != null)
            modelTransform.localRotation = Quaternion.Euler(0f, 0f, currentBank);
    }

    void HandleWheelAnimation()
    {
        if (frontWheelRotator != null)
            frontWheelRotator.Rotate(playerSpeed);

        if (rearWheelRotator != null)
            rearWheelRotator.Rotate(playerSpeed);
    }
}