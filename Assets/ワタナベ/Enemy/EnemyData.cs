using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObjectを使用して敵キャラのデータを定義するクラスを作成
/// </summary>
[CreateAssetMenu(fileName = "EnemyData")]
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
    public string behaviorType = "None";
}
