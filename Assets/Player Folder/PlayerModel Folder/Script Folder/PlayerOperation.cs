using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    public FrontWheelRotatorScript frontWheelRotator;
    public RearWheelRotatorScript rearWheelRotator;

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
    float brakePower = 50f;

    // 回転速度(左右)
    float turnSpeed = 100f;
    
    // 傾き（バンク）
    float bankAngle = 20f;
    float bankLerpSpeed = 5f;
    float currentBank = 0f;
    float targetBank = 0f;

    




    [SerializeField] LayerMask groundLayer; // 地面レイヤーを指定


    // Start is called before the first frame update
    void Start()
    {
        //初期化
        playerPosition = new Vector3(0.0f, 0.0f, 0.0f);
        playerRotation = new Vector3(30.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤー
        MovePlayer();      

        StickToGround();

        //タイヤの回転処理
        if (frontWheelRotator != null)
        {
            frontWheelRotator.Rotate(playerSpeed);
        }
        if (rearWheelRotator != null)
        {
            rearWheelRotator.Rotate(playerSpeed);
        }

    }

  

    //プレイヤー操作
    void MovePlayer()
    {
        if (Input.GetKey(KeyCode.W))
        {
            playerSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            playerSpeed -= deceleration * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.S))
        {
            playerSpeed -= brakePower * Time.deltaTime;
        }

        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) turn = -1f;
        else if (Input.GetKey(KeyCode.D)) turn = 1f;

        transform.Rotate(0f, turn * turnSpeed * Time.deltaTime, 0f);

        targetBank = Mathf.Lerp(targetBank, -turn * bankAngle, Time.deltaTime * bankLerpSpeed);
        currentBank = Mathf.Lerp(currentBank, targetBank, Time.deltaTime * bankLerpSpeed);

        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, currentBank);
        transform.rotation = targetRotation;

        transform.position += transform.forward * playerSpeed * Time.deltaTime;
       
    }

   

    

   

    // 地面に吸着させる処理
    void StickToGround()
    {
        if (!IsGrounded()) return;

        Ray ray = new Ray(transform.position + Vector3.up * 1f, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f, groundLayer))
        {
            Vector3 targetPosition = transform.position;
            targetPosition.y = hit.point.y;
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

            // 地面の傾きに合わせて回転（オプション）
            Quaternion groundRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, groundRotation, Time.deltaTime * 5f);
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.0f);
    }
}


