using System.Collections.Generic;
using UnityEngine;

public class RouteController : MonoBehaviour
{
    [Header("ルートマネージャー参照")]
    private RouteManager routeManager;

    // 現行ルートデータ
    private CatmullRomRoute currentRoute;
    // 使用ルート番号
    private int currentRouteIndex = 0;
    // 現在の距離
    public float currentDistance = 0f;

    private void Start()
    {
       // ルートマネージャーを取得
        if (routeManager == null)
        {
            routeManager = FindObjectOfType<RouteManager>();
            if (routeManager == null)
            {
                UnityEngine.Debug.LogError("ルートマネージャーが見つかりません。シーンにRouteManagerを配置してください。");
                return;
            }
        }
        // 初期化時にランダムなルートを設定
        InitWithRandomRoute();
    }

    public void Advance(float speed, float deltaTime)
    {
        //if (currentRoute==null) InitWithRandomRoute();
        currentDistance += speed * deltaTime;
        currentRoute.Advance(currentDistance);
    }


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
        if (routeManager == null)
        {
            //UnityEngine.Debug.LogError("ルートマネージャーが設定されていません。");
            return;
        }
        
        currentRoute = routeManager.GetRoute(routeIndex);
        
        if(currentRoute == null)
        {
            //UnityEngine.Debug.LogError("指定されたルートが存在しません。ルートインデックスを確認してください。");
            return;
        }
        //UnityEngine.Debug.Log("ルート 初期化成功");
    }

    // 初期化時にランダムなルートを設定する
    public void InitWithRandomRoute()
    {
        if (routeManager == null)
        {
            UnityEngine.Debug.LogError("ルートマネージャーが設定されていません。");
            return;
        }

        currentRoute = routeManager.GetRandomRoute();
        
        if(currentRoute == null)
        {
            UnityEngine.Debug.LogError("ランダムルートの取得に失敗しました。ルートが存在しない可能性があります。");
            return;
        }

        UnityEngine.Debug.Log("ランダムルート 初期化成功");
    }

    public CatmullRomRoute GetCurrentRoute()
    {
        if (currentRoute == null)
        {
            UnityEngine.Debug.LogError("現在のルートが設定されていません。InitWithRouteまたはInitWithRandomRouteを呼び出してください。");
            return null;
        }
        return currentRoute;
    }

    // ルートを変更する関数
    public void ChangeRoute(int newRouteIndex)
    {
        currentRouteIndex = newRouteIndex;
        InitWithRoute(newRouteIndex);
    }

    public Vector3 GetDirection()
    {
        return currentRoute.GetDirection(currentDistance);
    }

}
