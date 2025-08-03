using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;
    private Stunt2 stunt2; 

    public Transform modelTransform;
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    // プレイヤーの現在速度
  　public float playerSpeed = 0f;
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


    [SerializeField]
    GoalScript goalScript;
    JumpScript jumpScript;


    bool wasGrounded = true;

    private float accelerationTimer = 0f;
    private float accelerationDuration = 2f;
    private bool isAccelerating = false;

    Vector3 moveDir;

    public float blinkDuration = 4f;     // 点滅する総時間
    public float blinkInterval = 0.5f;   // 点滅の間隔
                                         // public Renderer playerRenderer;      // プレイヤーの見た目
   private bool playeraccel = false;



    void Start()
    {
        goalScript = GameObject.Find("Player").GetComponent<GoalScript>();
        jumpScript = GameObject.Find("Player").GetComponent<JumpScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");

        
    }

    void Update()
    {
        // 現在のプレイヤーの位置を取得
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -4000f, 530f);
        pos.z = Mathf.Clamp(pos.z, -17090f, 19585f);
        transform.position = pos;


        if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        {
              // プレイヤーの入力処理
              HandleInput();
             
        }
        // ホイールの回転アニメーション処理（走行演出）
        HandleWheelAnimation();
        UpdateAcceleration();
       
        
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
        rotationY += turnY * turnSpeed * Time.deltaTime;

        // X/Y軸を含んだ回転を作成
        Quaternion baseRotation = Quaternion.Euler(jumpScript.rotationX, rotationY, 0f);

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
        moveDir = forward.normalized;

        // 回転反映（地形に合わせる）
        Quaternion slopeRotation = Quaternion.LookRotation(forward, groundNormal);
        transform.rotation = slopeRotation;

        if (!goalScript.IsGoal())
        {
            // 移動入力（W/S）
            if (Input.GetKey(KeyCode.W) && playeraccel == false)
                playerSpeed += acceleration * Time.deltaTime;
            else if (Input.GetKey(KeyCode.S))
                playerSpeed -= acceleration * Time.deltaTime;
            else
                playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);
        }

      

        
    }

    
    void HandleWheelAnimation()
    {
        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);
        // 移動反映
        transform.position += moveDir * playerSpeed * Time.deltaTime;


        if (gameManagerScript.IsGameStarted() && goalScript.IsGoal())
        {
            playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);
        }
    }

    public float GetPlayerSpeed()
    {
        return playerSpeed;
    }
    //ジャンプ成功
    public void Acceleration()
    {
        maxSpeed = 5f;
        playerSpeed = maxSpeed;
        // 一定時間たったら戻す（実際の処理は外で管理）
        accelerationTimer = accelerationDuration;
        isAccelerating = true;
    }

    void UpdateAcceleration()
    {
        if (isAccelerating == true)
        {
            accelerationTimer -= Time.deltaTime;
            if (accelerationTimer <= 0f)
            {
                maxSpeed = 3f; // 元に戻す値
                isAccelerating = false;
            }
        }
    }
    //敵の攻撃の当たり判定
    void OnCollisionEnter(Collision collision)
    {    //(CompareTag("EnemyAttack") -> "PlayerAttack"に変更
        if (collision.gameObject.CompareTag("EnemyAttack"))
        {
            playerSpeed = 0;
            //点滅処理
            StartCoroutine(BlinkCoroutine());
        }

       

    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {

            playeraccel = true;
            playerSpeed = 0;
            Debug.Log("atari");


        }
        else
        {
           playeraccel = false;
        }

    }

        //点滅処理
        IEnumerator BlinkCoroutine()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        int blinkCount = 10;
        float blinkInterval = 0.1f;

        for (int i = 0; i < blinkCount; i++)
        {
            foreach (Renderer r in renderers)
            {
                r.enabled = !r.enabled;
            }
            yield return new WaitForSeconds(blinkInterval);
        }

        // 最後はすべて表示状態に戻す
        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }
    }


}