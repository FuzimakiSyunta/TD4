using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    // 敵ステータス管理(性格・バイクの性能・体力・運転能力などのステータスを取得する)
    EnemyData enemyData;

    // 敵のルート管理(分岐や、ルート内の左右移動や加速の指示を行う)
    [Header("ルート制御 スクリプト")]
    public RouteController routeController;

    // 敵スタント管理(上記のステータスやルート情報から行うスタントを決定する)


    // 敵行動管理(移動・妨害・加速などの指示を受けて行動処理を実行する)


    // 敵のアニメーション管理(敵のアニメーションを制御する)


    private GameManager gameManagerScript;
    public GameObject gameManager;

    public Transform modelTransform;
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

    // 速度
    float moveSpeed = 0f;
    //加速
    public float acceleration = 35f;
    //減速
    public float deceleration = 50f;
    //最大速度
    public float maxSpeed = 600f;
    //ブレーキ時の減速度
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

    [Header("移動量")]
    public Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // 各項目のリセット処理

        // ランダムルートで初期化
        routeController.InitWithRandomRoute();



    }

    // Update is called once per frame
    void Update()
    {
        // ルート制御スクリプトが設定されている場合は、ルート制御の更新を行う
        if (routeController != null)
        {
            // -- 加速処理 -- //
            // 目標速度に向けて加速（今は常時加速）
            moveSpeed += acceleration * Time.deltaTime;
            moveSpeed = Mathf.Clamp(moveSpeed, 0f, maxSpeed);


            // -- ルート計算処理 -- //
            // ルート・移動量の更新
            routeController.Advance(moveSpeed, Time.deltaTime);
            // ルートに沿った移動方向を取得
            moveVec = routeController.GetDirection();


            // 実際の移動（速度ベース）
            Vector3 move = moveVec * moveSpeed * Time.deltaTime;
            transform.position += move;


            // -- 向き補正（徐々に回す）-- //
            Vector3 currentForward = transform.forward;

            // 地形法線
            Terrain terrain = Terrain.activeTerrain;
            Vector3 groundNormal = Vector3.up;
            if (terrain != null)
            {
                float normX = transform.position.x / terrain.terrainData.size.x;
                float normZ = transform.position.z / terrain.terrainData.size.z;
                groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
            }

            // スムーズな回転（角速度制限あり）
            float rotationSpeed = 3.0f; // ラジアン/秒
            Vector3 newForward = Vector3.RotateTowards(currentForward, moveVec, rotationSpeed * Time.deltaTime, 0f);

            // 地形に沿った回転を適用
            Quaternion slopeRotation = Quaternion.LookRotation(newForward, groundNormal);
            transform.rotation = slopeRotation;

            // --- 見た目の処理 ---
            if (frontWheelRotator != null)
                frontWheelRotator.Rotate(moveSpeed);

            if (rearWheelRotator != null)
                rearWheelRotator.Rotate(moveSpeed);
        

    }

        //HandleWheelAnimation();

        //// 移動系の処理(アニメーション含む)

        //// 回転入力（左右/Y軸）
        //float turnY = 0f;
        //if (Mathf.Abs(moveSpeed) > 0.1f)
        //{
        //    if (Input.GetKey(KeyCode.A)) turnY = -1f;
        //    else if (Input.GetKey(KeyCode.D)) turnY = 1f;
        //}
        //rotationY += turnY * turnSpeed * Time.deltaTime;

        //// X/Y軸を含んだ回転を作成
        //Quaternion baseRotation = Quaternion.Euler(jumpScript.rotationX, rotationY, 0f);

        //// 地面の法線を取得（Terrain前提）
        //Terrain terrain = Terrain.activeTerrain;
        //Vector3 groundNormal = Vector3.up;
        //if (terrain != null)
        //{
        //    float normX = transform.position.x / terrain.terrainData.size.x;
        //    float normZ = transform.position.z / terrain.terrainData.size.z;
        //    groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
        //}

        //// 上下の傾きを含んだ forward 方向
        //Vector3 forward = baseRotation * Vector3.forward;

        //// 地形の傾斜に沿って補正（上下移動を許すなら ProjectOnPlane は使わない）
        //Vector3 moveDir = forward.normalized;

        //// 回転反映（地形に合わせる）
        //Quaternion slopeRotation = Quaternion.LookRotation(forward, groundNormal);
        //transform.rotation = slopeRotation;

    }

    void HandleWheelAnimation()
    {
        if (frontWheelRotator != null)
            frontWheelRotator.Rotate(moveSpeed);

        if (rearWheelRotator != null)
            rearWheelRotator.Rotate(moveSpeed);
    }


}
