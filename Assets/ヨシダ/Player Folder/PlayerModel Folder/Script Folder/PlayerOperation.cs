using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    //ゲームマネージャーの参照
    private GameManager gameManagerScript; // ゲームマネージャーのスクリプト参照
    public GameObject gameManager; // ゲームマネージャーのオブジェクト


    public Transform modelTransform; // モデル（見た目）だけを傾ける
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    float playerSpeed = 0f;
    float acceleration = 50f;
    float deceleration = 50f;
    float maxSpeed = 200f;
    float brakePower = 50f;

    float turnSpeed = 100f;
    float rotationY = 0f;

    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

    GoalScript goalScript;
  
    void Start()
    {
        // stunt = GetComponent<Stunt>();
        goalScript = GameObject.Find("bike body 1").GetComponent<GoalScript>();
        // ゲームマネージャーの参照を取得
        if (gameManager != null)
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManagerが設定されていません。");
        }
    }
        

    void Update()
    {
        Vector3 pos = transform.position;



        //座標制御
        // X軸の制限（-5 ～ 5）
        pos.x = Mathf.Clamp(pos.x, -2538f,13699f);
        // Y軸の制限（0 ～ 10）
        pos.z = Mathf.Clamp(pos.z, -3270f, 3663f);

        transform.position = pos;
        if(gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
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

        // 前進
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;

        // 後退
        else if (Input.GetKey(KeyCode.S))
            playerSpeed -= acceleration * Time.deltaTime;

        // 何も押してないときに自然減速
        else
            playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);

        // プレイヤーの速度をクランプ（後退も許可）
        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);
    }

    void HandleMovement()
    {
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

        // ★移動方向にWallタグのオブジェクトがあるかチェック
        Vector3 checkDir = playerSpeed >= 0 ? moveDir : -moveDir;
        float checkDistance = Mathf.Abs(playerSpeed) * Time.deltaTime + 0.1f;

        if (Physics.Raycast(transform.position, checkDir, out RaycastHit wallHit, checkDistance))
        {
            if (wallHit.collider.CompareTag("Wall"))
            {
                playerSpeed = 0f; // スピードも止める
                return; // 移動しない
            }
        }

        // 壁がないので移動実行
        transform.position += moveDir * playerSpeed * Time.deltaTime;
    }


    void HandleBankRotation()
    {
        float turn = 0f;
        if (playerSpeed >=10000) 
        {
            if (Input.GetKey(KeyCode.A))
            {
                turn = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                turn = 1f;
            }
            targetBank = -turn * bankAngle;
            currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);
            if (modelTransform != null)
            {
                modelTransform.localRotation = Quaternion.Euler(0f, 0f, currentBank);
            }
        }

       
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