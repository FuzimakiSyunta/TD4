using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOperation : MonoBehaviour
{
    //プレイヤーのPosition
    Vector3  playerPosition = Vector3.zero;
   
    //プレイヤーの回転
    Vector3 playerRotation = Vector3.zero;

    //プレイヤースピード
    float playerSpeed = 0;
    
    //加速
    float acceleration = 10f;

    // 減速度
    float deceleration = 10f;

    float maxSpeed = 100f;

    //ブレーキの減速
    float brakePower = 20f;



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
    }

    //プレイヤー操作
    void MovePlayer()
    {
        //移動処理
        if (Input.GetKey(KeyCode.W))
        {
            //加速
            playerSpeed += acceleration * Time.deltaTime;
        }
        else
        {
            // 減速（キーを離したら自然に減速）
            playerSpeed -= deceleration * Time.deltaTime;
        }

        //ブレーキの処理
        if (Input.GetKey(KeyCode.S))
        {
            // ブレーキ（強制減速）
            playerSpeed -= brakePower * Time.deltaTime;
        }

        //スピードを制限
        playerSpeed = Mathf.Clamp(playerSpeed, 0f, maxSpeed);

        //前方向に進む処理
        playerPosition.z += playerSpeed * Time.deltaTime;

        //左右に移動する処理
        if(Input.GetKey(KeyCode.D)) 
        {
            playerPosition.x +=  playerSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.A))
        {
            playerPosition.x -= playerSpeed * Time.deltaTime;
        }

        //位置の反映
        transform.position = playerPosition;
    }
}
