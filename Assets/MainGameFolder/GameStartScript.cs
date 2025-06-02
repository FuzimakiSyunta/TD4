using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム開始演出のためのスクリプト
public class GameStartScript : MonoBehaviour
{
    // 実行フレームによるタイミングのズレを抑えるため、
    // デルタタイムを使用する
    private float deltaTime = 0.0f;

    ///
    /// ゲーム開始時の演出を行うための変数
    ///

    // カウントダウンを行うかどうか
    public bool isStartCountDown = false;

    // カウントダウンの回数
    // カウント回数 * 下記のカウント発生時間だけカウントダウンが行われる
    [Header("カウントダウン回数")]
    public int countDownTime = 3;
    
    // 1カウントの時間を定義 
    [Header("1カウントあたりの表示時間")]
    public float oneSecond = 1.0f;
    // 現在のカウントの経過時間を定義
    private float currenOneSecond = 0.0f;

    // カウントダウン用のテキスト・UI配列
    [Header("カウントダウンテキスト")]
    public Text countDownText = null;

    //[Header("カウントダウン用のUI配列")]
    //public GameObject[] countDownUIArray = new GameObject[3];


    // Start is called before the first frame update
    void Start()
    {
        // 経過時間の初期化
        // 変数に合わせる(外部で設定可能)
        currenOneSecond = oneSecond;


    }

    // Update is called once per frame
    void Update()
    {
        // デルタタイムを更新
        deltaTime = Time.deltaTime;
     
        // カウントダウンが有効な場合
        if (isStartCountDown)
        {
            // カウントが最大値と同値の場合(カウント開始時や減少時)
            // カウントUI(またはログ)の表示を行う
            if (currenOneSecond == oneSecond)
            {
                // カウントダウンのテキストを有効化
                countDownText.gameObject.SetActive(true);

                // (ToDo)UIの表示を行い、更新を有効にする
                countDownText.text = countDownTime.ToString();

                // デバッグ用のログ出力
                Debug.Log("カウントダウン: " + countDownTime + "秒");
            }


           


            // 経過時間を更新
            currenOneSecond -= deltaTime;

            // 1秒経過したらカウントダウンを進める
            if (currenOneSecond <= 0.0f)
            {

                // カウントダウン時間を減らす
                countDownTime--;
                
                // 次の1秒の時間を設定
                currenOneSecond = oneSecond;

                // カウントダウンが終了した場合
                if (countDownTime <= 0)
                {
                    // カウントダウンのテキストを更新
                    countDownText.text = "GO!";

                    // デバッグ用のログ出力
                    Debug.Log("行きましょう");

                    // カウントダウンを無効にする
                    isStartCountDown = false;
                    // ゲーム開始の処理を行う
                    //StartGame();
                }

               

            }

        }

    }
}
