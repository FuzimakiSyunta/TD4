using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// ScriptableObjectを使用して敵キャラのデータを定義するクラスを作成
/// </summary>
[CreateAssetMenu(fileName = "EnemyData",menuName = "ScriptableObjects/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    // プレハブ
    public GameObject prefab;
    // 初期座標
    public Vector3 initialPosition;
    // 敵キャラの体力
    public int health;
    // 敵キャラの移動速度
    public float speed;
    // 敵キャラの行動タイプ(例:凶暴,規範的,平和主義者,チャレンジャー 等)
    public string behaviorType; 
   
}


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
            // 敵キャラのプレハブ・データを取得
            GameObject enemy = Instantiate(data.prefab, data.initialPosition, Quaternion.identity);
            // 生成した敵キャラを配列に追加
            spawnedEnemies.Add(enemy);

            UnityEngine.Debug.Log("敵キャラ" + i + "を生成しました: " + data.prefab.name + " 位置: " + data.initialPosition + " 体力: " + data.health + " 速度: " + data.speed + " 行動タイプ: " + data.behaviorType);

        }

    }

}
