using System.Runtime.CompilerServices;
using UnityEngine;

public class JCKPlayerOperation1 : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;
    private Stunt2 stunt2;

    public Transform modelTransform;
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    public float playerSpeed = 0f;
    public float acceleration = 35f;
    public float deceleration = 50f;
    public float maxSpeed = 600f;
    public float brakePower = 300f;

    float turnSpeed = 100f;
    float rotationY = 0f;

    [SerializeField]
    GoalScript goalScript;
    JumpScript jumpScript;

    bool wasGrounded = true;

    private float accelerationTimer = 0f;
    private float accelerationDuration = 0.2f;
    private bool isAccelerating = false;

    public bool IsAccelerating => isAccelerating;

    void Start()
    {
        goalScript = GameObject.Find("Player")?.GetComponent<GoalScript>();
        jumpScript = GameObject.Find("Player")?.GetComponent<JumpScript>();

        if (goalScript == null) Debug.LogError("goalScriptがnullです。PlayerにGoalScriptがついているか確認してください。");
        if (jumpScript == null) Debug.LogError("jumpScriptがnullです。PlayerにJumpScriptがついているか確認してください。");

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");
    }

    void Update()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -4000f, 530f);
        pos.z = Mathf.Clamp(pos.z, -17090f, 19585f);
        transform.position = pos;

        HandleInput();
        HandleWheelAnimation();
        UpdateAcceleration();
    }

    void HandleInput()
    {
        if (goalScript == null || jumpScript == null) return;

        float turnY = 0f;
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) turnY = -1f;
            else if (Input.GetKey(KeyCode.D)) turnY = 1f;

            if (JCScript.Instance != null && JCScript.Instance.LeftStick.x != 0)
            {
                turnY = JCScript.Instance.LeftStick.x;
            }
        }
        rotationY += turnY * turnSpeed * Time.deltaTime;

        Quaternion baseRotation = Quaternion.Euler(jumpScript.rotationX, rotationY, 0f);

        // 地形法線の取得（Terrainがnullになるビルド対策あり）
        Terrain terrain = Terrain.activeTerrain;
        Vector3 groundNormal = Vector3.up;
        if (terrain != null)
        {
            float normX = transform.position.x / terrain.terrainData.size.x;
            float normZ = transform.position.z / terrain.terrainData.size.z;
            groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
        }
        else
        {
            Debug.LogWarning("ビルド時に Terrain.activeTerrain が null です");
        }

        Vector3 forward = baseRotation * Vector3.forward;
        Vector3 moveDir = forward.normalized;

        // 通常通り回転を上書き（変化なし）
        transform.rotation = Quaternion.LookRotation(forward, groundNormal);
        // もし回転のブレが気になるなら以下に差し替えてもOK（挙動は変わらずスムーズになる）
        // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(forward, groundNormal), 10f * Time.deltaTime);

        if (!goalScript.IsGoal())
        {
            bool forwardInput = Input.GetKey(KeyCode.W);
            bool backwardInput = Input.GetKey(KeyCode.S);

            if (JCScript.Instance != null)
            {
                forwardInput = forwardInput || JCScript.Instance.RightZRButton;
                backwardInput = backwardInput || JCScript.Instance.LeftZLButton;
            }

            if (forwardInput)
            {
                playerSpeed += acceleration * Time.deltaTime;
            }
            else if (backwardInput)
            {
                playerSpeed -= acceleration * Time.deltaTime;
            }
            else
            {
                playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);
            }
        }

        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);
        transform.position += moveDir * playerSpeed * Time.deltaTime;
    }

    void HandleWheelAnimation()
    {
        //if (frontWheelRotator != null)
        //    frontWheelRotator.Rotate(playerSpeed);

        //if (rearWheelRotator != null)
        //    rearWheelRotator.Rotate(playerSpeed);
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }

    public void Acceleration()
    {
        maxSpeed = 5f;
        playerSpeed = maxSpeed;
        accelerationTimer = accelerationDuration;
        isAccelerating = true;
    }

    void UpdateAcceleration()
    {
        if (isAccelerating)
        {
            accelerationTimer -= Time.deltaTime;
            if (accelerationTimer <= 0f)
            {
                maxSpeed = 3f;
                isAccelerating = false;
            }
        }
    }
}
