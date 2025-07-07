using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pointerMoveScript : MonoBehaviour
{
    private GameManager gameManagerScript;
    public GameObject gameManager;

    private PlayerOperation playerOperationScript;
    public GameObject playerOperation;

    public float moveSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerOperationScript = playerOperation.GetComponent<PlayerOperation>();
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager"); // 名前に注意！
        }

        if (gameManager != null)
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManager オブジェクトが見つかりませんでした！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOperationScript == null)
        {
            Debug.LogWarning("playerOperationScript が null です");
            return;
        }

        moveSpeed= playerOperationScript.GetPlayerSpeed();

        // 移動入力
        if (Input.GetKey(KeyCode.W))
            moveSpeed -= Time.deltaTime;
        else if (Input.GetKey(KeyCode.S))
            moveSpeed += Time.deltaTime;

        // 実際に移動する処理を追加（Z軸に移動する場合）
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }
}
