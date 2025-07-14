using System.Collections.Generic;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
    [Header("ルートデータ（ScriptableObject）")]
    public List<RouteData> routeDatas;

    // 制御点データのリストを返す（必要な形式で渡す）
    public List<Vector3> GetRoutePoints(int index)
    {
        if (index < 0 || index >= routeDatas.Count) return null;
        return routeDatas[index].controlPoints;
    }

    // ランダムルート取得
    public List<Vector3> GetRandomRoutePoints()
    {
        if (routeDatas.Count == 0) return null;
        int randomIndex = Random.Range(0, routeDatas.Count);
        return GetRoutePoints(randomIndex);
    }
}
