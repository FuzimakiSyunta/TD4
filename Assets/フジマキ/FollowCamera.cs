using UnityEngine;

public class FollowBehindCamera : MonoBehaviour
{
    public Transform target;       // プレイヤーなどのターゲット
    public float distance = 5f;    // プレイヤーとの距離
    public float height = 2f;      // カメラの高さ
    public float smoothSpeed = 5f; // 滑らかさ

    void LateUpdate()
    {
        if (target == null) return;

        // プレイヤーの向いている方向の「後ろ」にオフセットを取る
        Vector3 behindPosition = target.position - target.forward * distance + Vector3.up * height;

        // カメラを滑らかに移動させる
        transform.position = Vector3.Lerp(transform.position, behindPosition, smoothSpeed * Time.deltaTime);

        // プレイヤーの方向を見る
        transform.LookAt(target);
    }
}
