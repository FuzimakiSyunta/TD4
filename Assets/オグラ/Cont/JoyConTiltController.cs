using UnityEngine;

public class JoyConTiltController : MonoBehaviour
{
    public bool isLeftJoyCon;  // 左Joy-Conならtrue、右Joy-Conならfalse
    private Joycon joycon;

    void Start()
    {
        foreach (var jc in JoyconManager.Instance.j)
        {
            if (jc.isLeft == isLeftJoyCon)
            {
                joycon = jc;
                break;
            }
        }

        if (joycon == null)
        {
            Debug.LogError("指定したJoy-Conが見つかりませんでした。");
        }
    }

    void Update()
    {
        if (joycon == null) return;

        // Joy-Conの加速度データを取得
        Vector3 accel = joycon.GetAccel();

        // ピッチとロールの計算（加速度から算出）
        float pitch = Mathf.Atan2(accel.y, accel.z) * Mathf.Rad2Deg;
        float roll = Mathf.Atan2(accel.x, accel.z) * Mathf.Rad2Deg;

        // クォータニオン回転として変換
        Quaternion joyconRotation = Quaternion.Euler(-pitch, 0f, roll);

        // 回転を滑らかに反映
        transform.rotation = Quaternion.Slerp(transform.rotation, joyconRotation, Time.deltaTime * 5f);
    }
}
