using System.Collections.Generic;
using UnityEngine;

public class RouteController : MonoBehaviour
{
    [Header("ルートマネージャー参照")]
    public RouteManager routeManager;

    // 現行ルートデータ
    private CatmullRomRoute currentRoute;

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

   

    public void Advance(float speed,float deltaTime)
    {
        if (!initialized) return;

        currentDistance += speed * deltaTime;
        Vector3 pos = GetPositionByDistance(currentDistance);
        Vector3 next = GetPositionByDistance(currentDistance + 1f);
        transform.forward = (next - pos).normalized;
    }

    // ルートを変更する関数
    public void ChangeRoute(int newRouteIndex)
    {
        currentRouteIndex = newRouteIndex;
        InitWithRoute(newRouteIndex);
        currentDistance = 0f; // 進行距離リセット（継続したい場合は工夫可能）
    }

    // 進行距離に基づいて位置を取得する関数
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


}
