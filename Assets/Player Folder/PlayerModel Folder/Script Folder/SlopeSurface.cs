using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlopeSurface : MonoBehaviour
{
    public Transform frontWheel;
    public float wheelRadius = 0.35f;
    public LayerMask groundLayer;   // 追加: 地面レイヤーを設定できるように

    private float frontWheelAngle = 10f;
    public float groundCheckDistance = 0.5f; // Rayの距離
    // 坂の角度（地面の法線からの傾き）
    public float slopeAngle { get; private set; }

    private Vector3 groundNormal = Vector3.up;

    private void Start()
    {
        UpdateSlopeAngle();
    }

    // 地面の法線を更新して傾きを計算する（手動呼び出し可能）
    public void UpdateSlopeAngle()
    {
        Collider col = GetComponent<Collider>();

        // コライダーの向きや形状による法線の取り方は状況に応じて変える必要あり
        // ここでは単純に上方向と仮定
        groundNormal = transform.up;
        slopeAngle = Vector3.Angle(groundNormal, Vector3.up);
    }

    // 速度補正用の倍率など返すメソッドも用意可能
    public float GetSpeedMultiplier()
    {
        // 例: 傾きが大きいほど減速する（坂が急なら速度半分にするイメージ）
        return Mathf.Clamp01(1f - (slopeAngle / 45f));
    }

    public void Rotate(float speed)
    {
        // タイヤの回転処理
        float circumference = 2f * Mathf.PI * wheelRadius;
        float distance = speed * Time.deltaTime;
        float deltaAngle = (distance / circumference) * 360f;

        frontWheelAngle += deltaAngle;

        if (frontWheel != null)
            frontWheel.localRotation = Quaternion.Euler(frontWheelAngle, 0f, 0f);

        // 接地判定（Raycast）
        if (frontWheel != null)
        {
            Ray ray = new Ray(frontWheel.position, Vector3.down);
            bool isGrounded = Physics.Raycast(ray, groundCheckDistance, groundLayer);

            // 例：接地していればログ出力（必要に応じて処理を追加してください）
            if (isGrounded)
            {
                Debug.Log("Front wheel is on the ground.");
            }
            else
            {
                Debug.Log("Front wheel is NOT on the ground.");
            }
        }
    }
}
