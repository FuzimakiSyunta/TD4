using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{

    // 敵ステータス管理(性格・バイクの性能・体力・運転能力などのステータスを取得する)


    // 敵のルート管理(分岐や、ルート内の左右移動や加速の指示を行う)


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
        // 仮で  自身を前方に移動する
        transform.Translate(Vector3.forward * Time.deltaTime * 30f);

    }
}
