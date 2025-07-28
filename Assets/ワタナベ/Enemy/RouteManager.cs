using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class RouteManager : MonoBehaviour
{
    [Header("ルートデータ（ScriptableObject）")]
    public List<RouteData> routeDatas;

    public void Start()
    {
        // ルートデータが設定されていない場合は初期化
        if (routeDatas == null)
        {
            routeDatas = new List<RouteData>();
        }
    }

    public void Update()
    {
        // ルートデータの更新(ギズモ関係)


    }


    //
    // -- 生成関係処理 -- //
    //

    // 全ルート生成関数
    public void BakeAllRoute()
    {
        foreach (var routeData in routeDatas)
        {
            BakeRoute(routeData);
        }

    }

    // ルート生成関数
    public void BakeRoute(RouteData routeData)
    {
        if (routeData == null || routeData.controlPoints == null || routeData.controlPoints.Count < 4)
        {
            Debug.LogError("制御点が不足しています");
            return;
        }
        // サンプリングされたポイントと累積距離のリストを初期化
        List<Vector3> sampledPoints = new List<Vector3>();
        List<float> cumulativeDistances = new List<float>();
        float distanceSum = 0f;
        int samplesPerSegment = 20; // サンプリング数
        for (int i = 0; i < routeData.controlPoints.Count - 3; i++)
        {
            for (int j = 0; j <= samplesPerSegment; j++)
            {
                float t = j / (float)samplesPerSegment;
                Vector3 point = CatmullRom.Evaluate(
                    routeData.controlPoints[i],
                    routeData.controlPoints[i + 1],
                    routeData.controlPoints[i + 2],
                    routeData.controlPoints[i + 3],
                    t
                );
                sampledPoints.Add(point);
                if (j > 0)
                {
                    distanceSum += Vector3.Distance(sampledPoints[sampledPoints.Count - 2], point);
                }
                cumulativeDistances.Add(distanceSum);
            }
        }
        // サンプリングされたポイントと累積距離をルートデータに保存
        //routeData.sampledPoints = sampledPoints;
        //routeData.cumulativeDistances = cumulativeDistances;
    }


    //
    // -- 更新関係処理 -- //
    //







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


// ルートの制御点を設定する際の手間を減らすエディタ
[CustomEditor(typeof(RouteData))]
public class RouteDataEditor : Editor
{
    // ルートデータの制御点を表示するためのカスタムエディタ
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RouteData routeData = (RouteData)target;
        if (GUILayout.Button("Add Control Point"))
        {
            Undo.RecordObject(routeData, "Add Control Point");
            routeData.controlPoints.Add(Vector3.zero); // 新しい制御点を追加
            EditorUtility.SetDirty(routeData);
        }
        if (GUILayout.Button("Clear Control Points"))
        {
            Undo.RecordObject(routeData, "Clear Control Points");
            routeData.controlPoints.Clear(); // 制御点をクリア
            EditorUtility.SetDirty(routeData);
        }

       

    }

    // 制御点の位置をドラッグで変更できるようにする
    private void OnSceneGUI()
    {
        RouteData routeData = (RouteData)target;
        for (int i = 0; i < routeData.controlPoints.Count; i++)
        {
            Vector3 point = routeData.controlPoints[i];
            EditorGUI.BeginChangeCheck();
            Vector3 newPoint = Handles.PositionHandle(point, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(routeData, "Move Control Point");
                routeData.controlPoints[i] = newPoint;
                EditorUtility.SetDirty(routeData);
            }
        }
    }

    // 制御点リストをドラッグアンドドロップで編集可能にする
    private void OnEnable()
    {
        RouteData routeData = (RouteData)target;
        if (routeData.controlPoints == null)
        {
            routeData.controlPoints = new List<Vector3>();
        }
    }

}