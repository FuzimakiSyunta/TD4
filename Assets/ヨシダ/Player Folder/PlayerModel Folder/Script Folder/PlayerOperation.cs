using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;

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

    float bankAngle = 10f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;
    float slopeAngle =0f;
   [SerializeField]
    GoalScript2 goalScript;
   
    bool wasGrounded = true;

    void Start()
    {
        goalScript = GameObject.Find("Player").GetComponent<GoalScript2>();

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

        //if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        //{
            HandleInput();
         
            HandleBankRotation();
            HandleWheelAnimation();
            HandleMovement();
        // }
    }

    void HandleInput()
    {
        // 入力によるY回転更新
        float turn = 0f;
        if (Mathf.Abs(playerSpeed) > 0.1f)
        {
            if (Input.GetKey(KeyCode.A)) turn = -1f;
            else if (Input.GetKey(KeyCode.D)) turn = 1f;
        }
        rotationY += turn * turnSpeed * Time.deltaTime;

        // ベースのY回転
        Quaternion baseRotation = Quaternion.Euler(0f, rotationY, 0f);

        // Terrainがある前提で法線取得
        Terrain terrain = Terrain.activeTerrain;
        Vector3 groundNormal = Vector3.up;
        if (terrain != null)
        {
            float normX = transform.position.x / terrain.terrainData.size.x;
            float normZ = transform.position.z / terrain.terrainData.size.z;
            groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
        }

        // 地形法線に沿った前方向に補正
        Vector3 moveDir = Vector3.ProjectOnPlane(baseRotation * Vector3.forward, groundNormal).normalized;

        // 傾斜に沿った回転を作る
        Quaternion slopeRotation = Quaternion.LookRotation(moveDir, groundNormal);

        // 回転を反映
        transform.rotation = slopeRotation;

        // 移動入力
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            playerSpeed -= acceleration * Time.deltaTime;
        else
            playerSpeed = Mathf.MoveTowards(playerSpeed, 0f, deceleration * Time.deltaTime);

        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);

        // 傾斜に沿った方向で移動
        transform.position += moveDir * playerSpeed * Time.deltaTime;

    }

    void HandleMovement()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        RaycastHit hit;

        float slopeThreshold = 5f;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            Vector3 groundNormal = hit.normal;
            slopeAngle = Vector3.Angle(groundNormal, Vector3.up);

            if (slopeAngle > slopeThreshold)
            {
                Debug.Log("坂です！ 傾き: " + slopeAngle);
              



            }
            else
            {
                Debug.Log("平坦な地面です");

                // 平地なのでX軸の傾きだけを0に戻す
                Vector3 currentEuler = transform.rotation.eulerAngles;
                Quaternion targetRotation = Quaternion.Euler(0f, currentEuler.y, currentEuler.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }
      
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
            modelTransform.localRotation = Quaternion.Euler(0, 0f, currentBank);
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