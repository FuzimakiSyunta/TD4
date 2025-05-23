using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearWheelRotatorScript : MonoBehaviour
{
    // 後輪のTransform（回転させる対象のオブジェクト）
    public Transform rearWheel;
    
    // ホイールの半径（回転量の計算に使用）
    public float wheelRadius = 0.35f;
   
    // 後輪の現在の回転角（X軸の角度を保持）
    private float rearWheelAngle = 0f;

    public void Rotate(float speed)
    {
        // ホイールの円周（2πr）を計算
        float circumference = 2f * Mathf.PI * wheelRadius;
        
        // 今フレームで進んだ距離（速度 × 経過時間）
        float distance = speed * Time.deltaTime;
       
        // この距離に相当する回転角度（度数）を算出
        float deltaAngle = (distance / circumference) * 360f;

        // 現在の回転角に加算（フレームごとに累積）
        rearWheelAngle += deltaAngle;

        // 後輪のTransformにX軸回転を適用（Z軸・Y軸は回さない）
        if (rearWheel != null)
            rearWheel.localRotation = Quaternion.Euler(rearWheelAngle, 0f, 0f);
    }
}
