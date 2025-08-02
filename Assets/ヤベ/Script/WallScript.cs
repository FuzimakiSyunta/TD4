using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private PlayerOperation playerOperation;
    public Vector3 inputDir;
    // Start is called before the first frame update
    void Start()
    {
        playerOperation = GetComponent<PlayerOperation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.tag == "Wall")
        //{
        //    playerOperation.playerSpeed = 0;
        //    Debug.Log("壁に当たった");
        //}

        if (other.tag == "Wall")
        {
            // プレイヤーの進行方向を取得（前方ベクトル）
            Vector3 forward = transform.forward;

            // キー入力方向をベクトル化
             inputDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) inputDir = forward;
            else if (Input.GetKey(KeyCode.S)) inputDir = -forward;

            // 壁との接触点を取得
            Vector3 wallPoint = other.ClosestPoint(transform.position);
            Vector3 contactDir = (wallPoint - transform.position).normalized;

            // 入力方向と接触方向のDot積で判定
            float dot = Vector3.Dot(inputDir, contactDir);

            if (dot > 0.01f)
            {
                // 入力方向と壁方向が一致 → 押し付けている → 停止
                playerOperation.playerSpeed = 0;
                Debug.Log("押し付けている方向 → 停止");
            }
            else
            {
                // 離れる方向 → 通過可能
                Debug.Log("壁に接触中だが離れる方向 → 動ける");
            }
        }



        //if (other.tag == "Wall")
        //{
        //    Vector3 playerPos = transform.position;
        //    Vector3 wallPos = other.ClosestPoint(playerPos);

        //    // 壁 → プレイヤーのベクトル
        //    Vector3 wallToPlayer = (playerPos - wallPos).normalized;

        //    // 入力方向ベクトル（Z軸前後）
        //   inputDir = new Vector3(0f, 0f,
        //        Input.GetKey(KeyCode.W) ? 1f : (Input.GetKey(KeyCode.S) ? -1f : 0f));

        //    // 入力方向と壁の接触方向がほぼ一致していたら止める（Dot積で判定）
        //    float dot = Vector3.Dot(inputDir, wallToPlayer);

        //    if (dot > 0.5f)
        //    {
        //        // 壁方向に進もうとしている → 停止
        //        playerOperation.playerSpeed = 0;
        //        Debug.Log("壁方向に進行中（Trigger）→ 停止");
        //    }
        //    else
        //    {
        //        // 壁と逆方向に入力されている → 移動可能
        //        Debug.Log("壁接触中だが逆向き入力 → 通過可能");
        //    }
        //}


    }
}
