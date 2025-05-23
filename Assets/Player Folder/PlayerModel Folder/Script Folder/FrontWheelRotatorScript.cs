using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontWheelRotatorScript : MonoBehaviour
{
    // プレイヤーの前輪オブジェクト（Transform）
    public Transform frontWheel;

    // 前輪の半径（ホイールの回転量計算に使用）
    public float wheelRadius = 0.35f;

    // 前輪の回転角度（X軸回転）
    private float frontWheelAngle = 10f;

   

    public void Rotate(float speed)
    {
        // ホイールの円周を計算（2πr）
        float circumference = 2f * Mathf.PI * wheelRadius;
        
        // 今フレームで進んだ距離を算出（速度 × 時間）
        float distance = speed * Time.deltaTime;
       
        // 移動距離に基づいてホイールが回転する角度を計算（度数）
        float deltaAngle = (distance / circumference) * 360f;

        // ホイールの現在の回転角に加算（どんどん回る）
        frontWheelAngle += deltaAngle;


        // ホイールのTransformに回転を適用（X軸回転）
        if (frontWheel != null)
            frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);
    }


}
