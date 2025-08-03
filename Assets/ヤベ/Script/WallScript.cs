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

    }
}
