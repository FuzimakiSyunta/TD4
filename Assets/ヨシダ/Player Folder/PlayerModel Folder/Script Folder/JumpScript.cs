using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpScript : MonoBehaviour
{
    private Rigidbody rb;
    public float rotationX = 0f; // ← X軸回転角を保持
    float jumpForce = 20f; // ジャンプの強さ
    private float turnX;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // Rigidbodyを取得
    }

    // Update is called once per frame
    void Update()
    {
        rotationX += turnX * 50f * Time.deltaTime; // ピッチ速度
        rotationX = Mathf.Clamp(rotationX, -30f, 30f); // ピッチ制限
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
