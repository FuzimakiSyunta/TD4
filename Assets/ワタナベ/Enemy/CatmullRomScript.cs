using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(menuName = "RouteData")]
public class RouteData : ScriptableObject
{
    public List<Vector3> controlPoints;
    // ここに Boost, Jump などのメタ情報も追加可能
}


/// <summary>
/// Catmull-Rom 曲線に基づくルートを表現するクラス
/// </summary>
public class CatmullRomRoute : MonoBehaviour
{
    // ルートの制御点リスト
    private List<Vector3> controlPoints;
    // サンプリングされたポイントと累積距離のリスト
    private List<Vector3> sampledPoints = new();
    // 累積距離のリスト
    private List<float> cumulativeDistances = new();

    // 現在の距離
    private float currentDistance = 0f;
    // サンプリング数
    private int samplesPerSegment = 20;
    // 初期化フラグ
    private bool initialized = false;

    // Catmull-Rom 曲線の評価関数
    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    public void Setup(List<Vector3> points)
    {
        controlPoints = points;
        BakeRoute();
    }

    public void BakeRoute()
    {
        sampledPoints.Clear();
        cumulativeDistances.Clear();
        float distSum = 0f;

        for (int i = 0; i < controlPoints.Count - 3; i++)
        {
            for (int j = 0; j <= samplesPerSegment; j++)
            {
                float t = j / (float)samplesPerSegment;
                Vector3 point = CatmullRom(
                    controlPoints[i],
                    controlPoints[i + 1],
                    controlPoints[i + 2],
                    controlPoints[i + 3],
                    t
                );

                if (sampledPoints.Count > 0)
                    distSum += Vector3.Distance(point, sampledPoints[^1]);

                sampledPoints.Add(point);
                cumulativeDistances.Add(distSum);
            }
        }
    }

    // 移動量の取得
    public Vector3 GetDirection()
    {
        if (!initialized) return Vector3.zero;
        Vector3 posNow = GetPositionByDistance(currentDistance);
        Vector3 posNext = GetPositionByDistance(currentDistance + 1f);
        return (posNext - posNow).normalized;
    }

    // 進行距離に応じて位置を取得する関数
    public Vector3 GetPositionByDistance(float distance)
    {
        if (sampledPoints.Count == 0) return Vector3.zero;
        if (distance <= 0f) return sampledPoints[0];
        if (distance >= TotalDistance) return sampledPoints[^1];

        for (int i = 1; i < cumulativeDistances.Count; i++)
        {
            if (distance < cumulativeDistances[i])
            {
                float d0 = cumulativeDistances[i - 1];
                float d1 = cumulativeDistances[i];
                float t = Mathf.InverseLerp(d0, d1, distance);
                return Vector3.Lerp(sampledPoints[i - 1], sampledPoints[i], t);
            }
        }
        return sampledPoints[^1];
    }




    public float TotalDistance => cumulativeDistances.Count > 0 ? cumulativeDistances[^1] : 0f;

    // Gizmos描画用
    public void DrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 1; i < sampledPoints.Count; i++)
        {
            Gizmos.DrawLine(sampledPoints[i - 1], sampledPoints[i]);
        }

        Gizmos.color = Color.magenta;
        foreach (var p in controlPoints)
        {
            Gizmos.DrawSphere(p, 0.2f);
        }
    }

}
