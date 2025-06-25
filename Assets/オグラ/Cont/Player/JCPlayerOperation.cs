using UnityEngine;
using System.Collections.Generic; // Joyconクラスを使用するために必要

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

    // 新JoyCon: Joyconインスタンスを保持するリストと左右のJoy-Con参照
    private List<Joycon> joycons;
    private Joycon R_joycon;
    private Joycon L_joycon;

    void Start()
    {
        goalScript = GameObject.Find("bike body 1").GetComponent<GoalScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");

        // 新JoyCon: Joy-Conを初期化
        if (JoyconManager.Instance != null)
        {
            joycons = JoyconManager.Instance.j;
            foreach (var jc in joycons)
            {
                if (jc.isLeft)
                {
                    L_joycon = jc;
                }
                else
                {
                    R_joycon = jc;
                }
            }
            if (L_joycon == null) Debug.LogWarning("新JoyCon: 左Joy-Conが見つかりません。操作に影響が出る可能性があります。");
            if (R_joycon == null) Debug.LogWarning("新JoyCon: 右Joy-Conが見つかりません。操作に影響が出る可能性があります。");
        }
        else
        {
            Debug.LogError("新JoyCon: JoyconManager.Instance が見つかりません。Joy-Con操作は無効になります。");
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
            // 新JoyCon: ここで入力処理を直接行う
            HandlePlayerInput(); // 新しい入力処理メソッドを呼び出し

            HandleMovement();
            HandleBankRotation();
            HandleWheelAnimation();
        }
    }

    // 新JoyCon: プレイヤーの入力処理を一元化する新しいメソッド
    void HandlePlayerInput()
    {
        float currentTurnInput = 0f;
        float currentSpeedInput = 0f;

        // キーボード入力
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) currentTurnInput = -1f;
            else if (Input.GetKey(KeyCode.D)) currentTurnInput = 1f;
        }

        if (Input.GetKey(KeyCode.W))
            currentSpeedInput = 1f;
        else if (Input.GetKey(KeyCode.S))
            currentSpeedInput = -1f;

        // 新JoyCon: Joy-Con入力
        if (L_joycon != null)
        {
            float[] stick = L_joycon.GetStick();
            // 新JoyCon: スティックのX軸入力が検出されたら、キーボードの旋回入力を上書きする（または追加する）
            if (Mathf.Abs(stick[0]) > 0.1f) // デッドゾーンを設定
            {
                currentTurnInput = stick[0]; // スティック入力が検出されたら、スティックの値を優先
            }
        }

        // 新JoyCon: 右Joy-ConのZRボタンによる加速
        if (R_joycon != null && R_joycon.GetButton(Joycon.Button.SHOULDER_2))
        {
            currentSpeedInput = 1f; // ZRボタンが押されたら加速を優先
        }


        // 旋回の適用
        rotationY += currentTurnInput * turnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0f, rotationY, 0f);

        // 加速・減速の適用
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

        // 新JoyCon: 左Joy-Conスティックによるバンク入力
        if (L_joycon != null)
        {
            float[] stick = L_joycon.GetStick();
            if (Mathf.Abs(stick[0]) > 0.1f)
            {
                bankTurnInput = stick[0]; // スティック入力が検出されたら、スティックの値を優先
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