using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    [SerializeField]
    private JCKPlayerOperation jckPlayerOperation;
    public Vector3 inputDir;
    [SerializeField]
    private JCScript jcScript;
    // Start is called before the first frame update
    void Start()
    {
        jckPlayerOperation = GetComponent<JCKPlayerOperation>();
        jcScript = GameObject.Find("JCScriptObj").GetComponent<JCScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            // プレイヤーの進行方向を取得（前方ベクトル）
            Vector3 forward = transform.forward;

            // キー入力方向をベクトル化
            Vector3 inputDir = Vector3.zero;
            if (Input.GetKey(KeyCode.W) || jcScript.RightZRButton) inputDir = forward;
            else if (Input.GetKey(KeyCode.S) || jcScript.LeftZLButton) inputDir = -forward;

            // 衝突点（複数ある可能性があるが、最初のものを使用）
            ContactPoint contact = collision.contacts[0];
            Vector3 contactPoint = contact.point;

            // プレイヤーから接触点へのベクトル
            Vector3 contactDir = (contactPoint - transform.position).normalized;

            // 入力方向と接触方向のDot積で判定
            float dot = Vector3.Dot(inputDir, contactDir);

            if (dot > 0.01f)
            {
                // 押し付けている方向 → 停止
                jckPlayerOperation.playerSpeed = 0;
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
