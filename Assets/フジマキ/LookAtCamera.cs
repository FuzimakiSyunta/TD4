using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    void Update()
    {
        // カメラの位置を取得
        Vector3 cameraPos = Camera.main.transform.position;

        // 対象のオブジェクトの位置
        Vector3 targetPos = transform.position;

        // YとZを固定してX方向のみ差分を使う
        targetPos.x = cameraPos.x;

        // 自分の位置から見たターゲット方向を計算
        Vector3 direction = targetPos - transform.position;

        if (direction != Vector3.zero)
        {
            // LookRotationでX軸方向を向く回転を取得
            Quaternion rotation = Quaternion.LookRotation(direction);

            // X軸の回転だけ使うように制限（EulerでXだけ使う）
            transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
    }
}
