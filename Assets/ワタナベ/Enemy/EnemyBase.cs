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



    // Start is called before the first frame update
    void Start()
    {
        // 各項目のリセット処理



    }

    // Update is called once per frame
    void Update()
    {
        // ルート制御スクリプトが設定されている場合は、ルート制御の更新を行う
        if (routeController != null) {
            // ルート制御の更新処理を呼び出す
            routeController.Invoke("Update", Time.deltaTime);
            // ルート制御スクリプトから移動量を取得して、敵の位置を更新する
            Vector3 moveDirection = routeController.transform.forward; // 仮の移動方向を取得
            float moveSpeed = 30f; // 仮の移動速度を設定
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
        }

        // 仮で  自身を前方に移動する
        transform.Translate(Vector3.forward * Time.deltaTime * 30f);

    }
}
