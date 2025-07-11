using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    // 敵ステータス管理(性格・バイクの性能・体力・運転能力などのステータスを取得する)
    EnemyData enemyData;

    // 敵のルート管理(分岐や、ルート内の左右移動や加速の指示を行う)
    [Header("ルート制御 スクリプト")]
    public RouteController routeController;

    // 敵スタント管理(上記のステータスやルート情報から行うスタントを決定する)


    // 敵行動管理(移動・妨害・加速などの指示を受けて行動処理を実行する)


    // 敵のアニメーション管理(敵のアニメーションを制御する)

    [Header("移動量")]
    public Vector3 moveVec = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // 各項目のリセット処理

        // ランダムルートで初期化
        routeController.InitWithRandomRoute();



    }

    // Update is called once per frame
    void Update()
    {
        // ルート制御スクリプトが設定されている場合は、ルート制御の更新を行う
        if (routeController != null)
        {
            routeController.Advance(Time.deltaTime);

            moveVec = routeController.GetVelocity();
            transform.Translate(moveVec * Time.deltaTime, Space.World);
        }

    }
}
