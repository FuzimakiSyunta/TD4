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

    // 回転速度
    public float turnSpeed = 360f;
    float rotationY = 0f;

    float bankAngle = 10f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;
    float slopeAngle = 10f;

    [SerializeField]
    GoalScript goalScript;

    [SerializeField]
    JumpScript jumpScript;

    bool wasGrounded = true;

    [Header("移動量")]
    public Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // 各項目のリセット処理

        // ランダムルートで初期化
        routeController.InitWithRoute(0);
       

    }

    // Update is called once per frame
    void Update()
    {
        routeController.InitWithRoute(0);

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


            // -- 移動処理 -- //

            // 実際の移動（速度ベース）
            Vector3 move = moveVec * moveSpeed * Time.deltaTime;
            transform.position += move;


            // -- 回転処理 -- //

            // 方向を移動量から計算
            Quaternion targetRotation = Quaternion.LookRotation(moveVec, Vector3.up);
            // 回転をスムーズに行う
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed / 100f);


            //// -- 向き補正（徐々に回す）-- //
            //Vector3 currentForward = transform.forward;

            //// 地形法線
            //Terrain terrain = Terrain.activeTerrain;
            //Vector3 groundNormal = Vector3.up;
            //if (terrain != null)
            //{
            //    float normX = transform.position.x / terrain.terrainData.size.x;
            //    float normZ = transform.position.z / terrain.terrainData.size.z;
            //    groundNormal = terrain.terrainData.GetInterpolatedNormal(normX, normZ);
            //}

            //// スムーズな回転（角速度制限あり）
            //float rotationSpeed = 3.0f; // ラジアン/秒
            //Vector3 newForward = Vector3.RotateTowards(currentForward, moveVec, rotationSpeed * Time.deltaTime, 0f);

            //// 地形に沿った回転を適用
            //Quaternion slopeRotation = Quaternion.LookRotation(newForward, groundNormal);
            //transform.rotation = slopeRotation;

            // -- 見た目の処理 -- //

            // 前後ホイールのアニメーションを更新
            HandleWheelAnimation();
        
        }


    }

    void HandleWheelAnimation()
    {
        if (frontWheelRotator != null)
            frontWheelRotator.Rotate(moveSpeed);

        if (rearWheelRotator != null)
            rearWheelRotator.Rotate(moveSpeed);
    }


}
