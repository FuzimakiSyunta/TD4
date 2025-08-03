using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class pointerMoveScript : MonoBehaviour
{
    public Transform player; // プレイヤーのTransform
    public float fixedY = 50f; // 固定するY座標

    void Update()
    {
        if (player == null) return;

        // プレイヤーの位置に追従
        transform.position = player.position;

        Vector3 pos = transform.position;
        pos.y = fixedY;
        transform.position = pos;

        // Y軸の回転だけ180度に設定（X,Zはそのまま）
        Vector3 rot = transform.eulerAngles;
        rot.y = 180f;
        transform.eulerAngles = rot;

        // Y軸の回転だけ合わせる（X/Z回転は固定）
        transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);
    }
    void LateUpdate()
    {
        if (player == null) return;

        // プレイヤーのY軸回転に180度を加える（360度に収める）
        float yRotation = (player.eulerAngles.y + 180f) % 360f;

        // オブジェクトのY回転だけ設定（X,Zは保持）
        Vector3 rot = transform.eulerAngles;
        rot.y = yRotation;
        transform.eulerAngles = rot;
    }

}
