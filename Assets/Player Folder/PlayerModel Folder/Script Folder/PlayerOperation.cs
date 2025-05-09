using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    //プレイヤーのPosition
    Vector3 playerPosition = Vector3.zero;

    //プレイヤーの回転
    Vector3 playerRotation = Vector3.zero;

    //スピード
    float playerSpeed = 0f;
    //加速
    float acceleration = 100f;
    //減速
    float deceleration = 50f;
    
    //最高速度
    float maxSpeed = 500f;
    
    //ブレーキによる減速
    float brakePower = 20f;

    // 回転速度(左右)
    float turnSpeed = 100f;
    
    // 傾き（バンク）
    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;


    public Transform frontWheel;     // 前輪
    public Transform rearWheel;      // 後輪
   
    public float wheelRadius = 0.35f;

    private float frontWheelAngle = 0f;
    private float rearWheelAngle = 0f;

    // 外部からのジャンプ力
    private Vector3 externalVelocity = Vector3.zero;
  //  private Vector3 force;


    // Start is called before the first frame update
    void Start()
    {
        //初期化
        playerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        playerRotation = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー
        MovePlayer();

       

        RotateWheel();
    }

    //プレイヤー操作
    void MovePlayer()
    {
        // 加減速処理
        if (Input.GetKey(KeyCode.W))
            playerSpeed += acceleration * Time.deltaTime;
        else
            playerSpeed -= deceleration * Time.deltaTime;

        if (Input.GetKey(KeyCode.S))
            playerSpeed -= brakePower * Time.deltaTime;

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        // Y軸回転（左：Aキー、右：Dキー）
        float turn = 0f;
        if (Input.GetKey(KeyCode.A))
            turn = -1f;
        else if (Input.GetKey(KeyCode.D))
            turn = 1f;

        // 回転（左右） - Y軸回転
        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        // Z軸のバンク（傾き）を計算
        targetBank = Mathf.Lerp(targetBank, -turn * bankAngle, Time.deltaTime * bankLerpSpeed);

        // バンク（傾き）を反映
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        // Z軸のバンクを適用してプレイヤーの回転を設定
        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, currentBank);
        transform.rotation = targetRotation;

        // 前方に進む
        transform.position += transform.forward * playerSpeed * Time.deltaTime;

        // 外部加力（ジャンプなど）を加える
        transform.position += externalVelocity * Time.deltaTime;

        // 外部力を徐々に減衰（自然に落下するように）
        externalVelocity = Vector3.Lerp(externalVelocity, Vector3.zero, Time.deltaTime * 2f);

    }

    void RotateWheel()
    {
        // タイヤの円周 = 2πr
        float circumference = 2f * Mathf.PI * wheelRadius;

        // 進んだ距離 = 速度 × 時間
        float distance = playerSpeed * Time.deltaTime;

        // 回転角（度） = 進んだ距離 / 周長 × 360°
        float deltaAngle = (distance / circumference) * 360f;

        // 後輪・前輪ともに回転（X軸）
        rearWheelAngle += deltaAngle;
        frontWheelAngle += deltaAngle;

        rearWheel.localRotation = Quaternion.Euler(rearWheelAngle, 0f, 0f);
        frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);


    }

    internal void AddExternalForce(Vector3 force)
    {
        externalVelocity += force;
    }
}


