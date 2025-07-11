using UnityEngine;

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
    GoalScript2 goalScript;

    bool wasGrounded = true;
    float rotationX = 0f; // ← X軸回転角を保持
    public float jumpForce = 20f; // ジャンプの強さ

    private Rigidbody rb;

    void Start()
    {
        goalScript = GameObject.Find("Player").GetComponent<GoalScript>();

        if (gameManager != null)
            gameManagerScript = gameManager.GetComponent<GameManager>();
        else
            Debug.LogError("GameManagerが設定されていません。");

        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    void Update()
    {
        Vector3 pos = transform.position;

        transform.position = pos;

        //if (gameManagerScript.IsGameStarted() && !goalScript.IsGoal())
        //{
              HandleInput();
         
             
              HandleWheelAnimation();
             HandleMovement();
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
        rotationY += turnY * turnSpeed * Time.deltaTime;

        // 回転入力（上下/X軸）
        float turnX = 0f;
        if (Input.GetKey(KeyCode.UpArrow)) turnX = 1f;
        else if (Input.GetKey(KeyCode.DownArrow)) turnX = -1f;
        rotationX += turnX * 50f * Time.deltaTime; // ピッチ速度
        rotationX = Mathf.Clamp(rotationX, -30f, 30f); // ピッチ制限

        // X/Y軸を含んだ回転を作成
        Quaternion baseRotation = Quaternion.Euler(rotationX, rotationY, 0f);

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

        playerSpeed = Mathf.Clamp(playerSpeed, -maxSpeed * 0.5f, maxSpeed);

        // 移動反映
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
                //Debug.Log("平坦な地面です");

                //// 平地なのでX軸の傾きだけを0に戻す
                //Vector3 currentEuler = transform.rotation.eulerAngles;
                //Quaternion targetRotation = Quaternion.Euler(0f, currentEuler.y, currentEuler.z);
                //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                //rotationX = 0f;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Slope"))
        {
            rotationX = -18f;
            Debug.Log("坂です");
        }

        if (collision.gameObject.CompareTag("Jump"))
        {

            Debug.Log("ジャンプ");

            // ジャンプ前に縦の速度をリセット
            Vector3 velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;

            // 上＋前方向にジャンプ力を加える
            Vector3 jumpDirection = (Vector3.up + transform.forward * 0.3f).normalized;
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
        }

    }
}