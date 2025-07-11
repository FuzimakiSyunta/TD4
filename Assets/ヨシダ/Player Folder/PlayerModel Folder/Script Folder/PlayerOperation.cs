using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections.Generic;

public class PlayerOperation : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;

    public Transform modelTransform;
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    // プレイヤーの現在速度
    float playerSpeed = 0f;
    //加速
    public float acceleration = 35f;
    //減速
    public float deceleration = 50f;
    //最大速度
    public float maxSpeed = 600f;
    //ブレーキ時の原則
    public float brakePower = 300f;

    float turnSpeed = 100f;
    float rotationY = 0f;

    float bankAngle = 10f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;
    float slopeAngle = 10f;
    [SerializeField]
    GoalScript goalScript;
    JumpScript jumpScript;

    bool wasGrounded = true;
   

  

    void Start()
    {
        goalScript = GameObject.Find("Player").GetComponent<GoalScript>();
        jumpScript = GameObject.Find("Player").GetComponent<JumpScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");

        //Joy-Con初期化
        if (JCScript.Instance == null)
        {
            Debug.LogError("JCScript.Instance が見つかりません。Joy-Con操作は無効になります。プロジェクトにJCScriptをアタッチしたGameObjectを配置しているか確認してください。");
        }
    }

    void Update()
    {
        // 現在のプレイヤーの位置を取得
        Vector3 pos = transform.position;


        //if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        //{
              // プレイヤーの入力処理
              HandleInput();
              // ホイールの回転アニメーション処理（走行演出）
              HandleWheelAnimation();       
        // }
    }

    void HandleInput()
    {
        // 回転入力（左右/Y軸）
        float turnY = 0f;
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) turnY = -1f;
            else if (Input.GetKey(KeyCode.D)) turnY = 1f;
        }
        //Joy-Con入力
        if (JCScript.Instance != null)
        {
            //左Joy-ConスティックのX軸で旋回
            if (Mathf.Abs(JCScript.Instance.LeftStick.x) > 0.05f)
            {
                turnY = JCScript.Instance.LeftStick.x;
            }
        }
        rotationY += turnY * turnSpeed * Time.deltaTime;

        // X/Y軸を含んだ回転を作成
        Quaternion baseRotation = Quaternion.Euler(0.0f, rotationY, 0f);

        // 地面の法線を取得（Terrain前提）
        Terrain terrain = Terrain.activeTerrain;
        Vector3 groundNormal = Vector3.up;
        if (terrain != null)
        {
            float normX = transform.position.x / terrain.terrainData.size.x;
            float normZ = transform.position.z / terrain.terrainData.size.z;
            groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
        }

        // 上下の傾きを含んだ forward 方向
        Vector3 forward = baseRotation * Vector3.forward;

        // 地形の傾斜に沿って補正（上下移動を許すなら ProjectOnPlane は使わない）
        Vector3 moveDir = forward.normalized;

        // 回転反映（地形に合わせる）
        Quaternion slopeRotation = Quaternion.LookRotation(forward, groundNormal);
        transform.rotation = slopeRotation;

        // 移動入力（W/S）
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            playerSpeed -= acceleration * Time.deltaTime;
        else
            playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);

        if (JCScript.Instance != null)
        {
            //右Joy-ConのZRボタンで加速
            if (JCScript.Instance.RightZRButton)
            {
                playerSpeed += acceleration * Time.deltaTime;
            }
            //Joy-ConのSLボタン下がる
            else if (JCScript.Instance.LeftZLButton)
            {
                playerSpeed -= acceleration * Time.deltaTime;
            }
            else
            {
                playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);
            }
        }
        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);

        // 移動反映
        transform.position += moveDir * playerSpeed * Time.deltaTime;
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