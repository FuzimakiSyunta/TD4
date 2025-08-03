using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 敵キャラ管理クラス
/// </summary>
public class EnemyManager : MonoBehaviour
{

    [Header("ルート管理スクリプト")]
    public RouteManager routeManager;

    [Header("敵キャラのデータ一覧")]
    public EnemyData[] enemyDatas;

    [Header("生成した敵の参照")]
    public List<GameObject> spawnedEnemies = new List<GameObject>();

    // 敵の生成関数を実行する変数
    [Header("敵の生成関数を実行する変数(テスト用)")]
    public bool isSpawnEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        // Resourcesフォルダから全てのEnemyDataを自動で読み込む
        enemyDatas = Resources.LoadAll<EnemyData>("");

        if (enemyDatas == null || enemyDatas.Length == 0)
        {
            UnityEngine.Debug.LogError("敵キャラのデータが設定されていません。EnemyDataを設定してください。");
        }
        else
        {
            UnityEngine.Debug.Log($"敵キャラのデータが{enemyDatas.Length}件設定されました。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpawnEnemy)
        {
            // 敵キャラの生成を開始
            StartSetEnemys();
            // 生成処理が終わったらフラグをリセット
            isSpawnEnemy = false;
        }
    }

    /// <summary>
    /// シーン開始時などに敵キャラの生成を開始する処理
    /// </summary>
    public void StartSetEnemys()
    {
        // 敵の生成する数を配列の長さから取得
        int enemyCount = enemyDatas.Length;

        for (int i = 0; i < enemyCount; i++)
        {
            // 敵キャラのデータを取得
            EnemyData data = enemyDatas[i];
            if (data == null) continue; // nullチェック追加

            // 敵キャラのプレハブ・データを取得
            GameObject enemy = Instantiate(data.prefab, data.initialPosition, Quaternion.identity);
            // 生成した敵キャラを配列に追加
            spawnedEnemies.Add(enemy);

            UnityEngine.Debug.Log("敵キャラ" + i + "を生成しました: " + data.prefab.name + " 位置: " + data.initialPosition + " 体力: " + data.health + " 速度: " + data.speed + " 行動タイプ: " + data.behaviorType);
        }
    }
}
