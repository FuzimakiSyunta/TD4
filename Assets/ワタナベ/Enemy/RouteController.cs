using System.Collections.Generic;
using UnityEngine;

public class RouteController : MonoBehaviour
{
    [Header("ルートマネージャー参照")]
    public RouteManager routeManager;

    [Header("移動速度")]
    public float speed = 20f;

    // ルートの制御点リスト
    private List<Vector3> controlPoints;
    // サンプリングされたポイントと累積距離のリスト
    private List<Vector3> sampledPoints = new();
    // 累積距離のリスト
    private List<float> cumulativeDistances = new();

    private float currentDistance = 0f;
    private int samplesPerSegment = 20;
    private bool initialized = false;

    private int currentRouteIndex = 0;

    // 現在のルートインデックスを取得
    public int GetCurrentRouteIndex() => currentRouteIndex;



    // 次のルートに切り替え
    public void SwitchToNextRoute()
    {
        if (routeManager == null || routeManager.routeDatas.Count == 0) return;
        currentRouteIndex = (currentRouteIndex + 1) % routeManager.routeDatas.Count;
        ChangeRoute(currentRouteIndex);
    }

    // 前のルートに切り替え
    public void SwitchToPreviousRoute()
    {
        if (routeManager == null || routeManager.routeDatas.Count == 0) return;
        currentRouteIndex = (currentRouteIndex - 1 + routeManager.routeDatas.Count) % routeManager.routeDatas.Count;
        ChangeRoute(currentRouteIndex);
    }


    // 初期化時にルートを設定する
    public void InitWithRoute(int routeIndex)
    {
        controlPoints = routeManager.GetRoutePoints(routeIndex);
        if (controlPoints == null || controlPoints.Count < 4)
        {
            Debug.LogError("制御点が不足しています");
            return;
        }
        BakeRoute();
        initialized = true;
    }

    // 初期化時にランダムなルートを設定する
    public void InitWithRandomRoute()
    {
        controlPoints = routeManager.GetRandomRoutePoints();
        if (controlPoints == null || controlPoints.Count < 4)
        {
            Debug.LogError("制御点が不足しています");
            return;
        }
        BakeRoute();
        initialized = true;
    }

    private void BakeRoute()
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

    public void Advance(float deltaTime)
    {
        if (!initialized) return;

        currentDistance += speed * deltaTime;
        Vector3 pos = GetPositionByDistance(currentDistance);
        //transform.position = pos;

        Vector3 next = GetPositionByDistance(currentDistance + 1f);
        transform.forward = (next - pos).normalized;
    }

    public Vector3 GetVelocity()
    {
        if (!initialized) return Vector3.zero;
        Vector3 posNow = GetPositionByDistance(currentDistance);
        Vector3 posNext = GetPositionByDistance(currentDistance + 1f);
        return (posNext - posNow).normalized * speed;
    }

    public void ChangeRoute(int newRouteIndex)
    {
        currentRouteIndex = newRouteIndex;
        InitWithRoute(newRouteIndex);
        currentDistance = 0f; // 進行距離リセット（継続したい場合は工夫可能）
    }

    private Vector3 GetPositionByDistance(float distance)
    {
        if (sampledPoints.Count == 0) return Vector3.zero;
        if (distance <= 0f) return sampledPoints[0];
        if (distance >= cumulativeDistances[^1]) return sampledPoints[^1];

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 1; i < sampledPoints.Count; i++)
        {
            Gizmos.DrawLine(sampledPoints[i - 1], sampledPoints[i]);
        }

        Gizmos.color = Color.magenta;
        if (controlPoints != null)
        {
            foreach (var p in controlPoints)
            {
                Gizmos.DrawSphere(p, 0.2f);
            }
        }
    }
}
