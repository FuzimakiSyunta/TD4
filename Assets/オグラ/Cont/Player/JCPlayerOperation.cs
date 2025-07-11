using UnityEngine;
using System.Collections.Generic;

public class JCPlayerOperation : MonoBehaviour
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
        goalScript = GameObject.Find("Player").GetComponent<GoalScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");

        //JoyCon初期化
        if (JCScript.Instance == null)
        {
            Debug.LogError("JCScript.Instance が見つかりません。Joy-Con操作は無効になります。プロジェクトにJCScriptをアタッチしたGameObjectを配置しているか確認してください。");
        }
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -2538f, 1369f);
        pos.z = Mathf.Clamp(pos.z, -3270f, 3663f);
        transform.position = pos;

        if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        {
            HandlePlayerInput();

            HandleMovement();
            HandleBankRotation();
            HandleWheelAnimation();
        }
    }

    void HandlePlayerInput()
    {
        float currentTurnInput = 0f;
        float currentSpeedInput = 0f;

        //キーボード入力
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) currentTurnInput = -1f;
            else if (Input.GetKey(KeyCode.D)) currentTurnInput = 1f;
        }

        if (Input.GetKey(KeyCode.W))
            currentSpeedInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            currentSpeedInput = -1f;

        //Joy-Con入力
        if (JCScript.Instance != null)
        {
            //左Joy-ConスティックのX軸で旋回
            if (Mathf.Abs(JCScript.Instance.LeftStick.x) > 0.05f)
            {
                currentTurnInput = JCScript.Instance.LeftStick.x;
            }
            //右Joy-ConのZRボタンで加速
            if (JCScript.Instance.RightZRButton)
            {
                currentSpeedInput = 1f;//ZRボタンが押されたら加速を優先
            }
        }


        //旋回の適用
        rotationY += currentTurnInput * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        //加速・減速の適用
        if (currentSpeedInput > 0)
            playerSpeed += acceleration * Time.deltaTime;
        else if (currentSpeedInput < 0)
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
        transform.position += Vector3.up * 3f;
        Debug.Log("ジャンプ！");
    }

    void HandleBankRotation()
    {
        float bankTurnInput = 0f;

        if (Mathf.Abs(playerSpeed) > 5f)
        {
            if (Input.GetKey(KeyCode.A)) bankTurnInput = -1f;
            else if (Input.GetKey(KeyCode.D)) bankTurnInput = 1f;
        }

        //左Joy-ConスティックのX軸によるバンク入力
        if (JCScript.Instance != null)
        {
            if (Mathf.Abs(JCScript.Instance.LeftStick.x) > 0.05f)
            {
                bankTurnInput = JCScript.Instance.LeftStick.x;
            }
        }

        targetBank = -bankTurnInput * bankAngle;
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

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }
}