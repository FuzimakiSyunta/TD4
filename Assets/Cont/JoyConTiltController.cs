using UnityEngine;

public class JoyConTiltController : MonoBehaviour
{
    public bool isLeftJoyCon;  // 左Joy-Conならtrue、右Joy-Conならfalse
    private Joycon joycon;

    void Start()
    {
        // JoyconManagerのリストから左か右のJoy-Conを取得
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

        // Joy-Conの角度（クォータニオン）を取得
        Quaternion targetRotation = Quaternion.Euler(-roll, 0, pitch);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);

        // オブジェクトの回転に反映（必要に応じて調整）
        transform.rotation = joyconRotation;
    }
}
