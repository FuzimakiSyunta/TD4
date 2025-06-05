using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ゲーム開始演出のためのスクリプト
public class GameStartScript : MonoBehaviour
{
    //ゲームマネージャーの参照
    private GameManager gameManagerScript;
    public GameObject gameManager;

    // 実行フレームによるタイミングのズレを抑えるため、
    // デルタタイムを使用する
    private float deltaTime = 0.0f;

    // カウントダウンを行うかどうか
    private bool isStartCountDown = false;

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

    // カウントダウンが終了したか
    private bool endCountDown = false;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームマネージャーの参照を取得
        if (gameManager != null)
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }
        else
        {
            Debug.LogError("GameManagerが設定されていません。");
        }

        // 経過時間の初期化
        // 変数に合わせる(外部で設定可能)
        currenOneSecond = oneSecond;

        isStartCountDown = false; // カウントダウンを開始するためのフラグを初期化

    }

    // Update is called once per frame
    void Update()
    {
        bool game = gameManagerScript.IsGameStarted(); // ゲームが開始されているかどうかを取得
        if (!game)
        {
            isStartCountDown = true; // カウントダウンを開始するフラグを立てる
        }else
        {
            // ゲームが開始されている場合はカウントダウンを無効にする
            isStartCountDown = false;
        }

        // デルタタイムを更新
        deltaTime = Time.deltaTime;

        // カウントダウンが有効な場合
        if (isStartCountDown)
        {
            // カウントダウンの実行
            UpdateCountDown();
        }

        if (endCountDown)
        {

            // テキストの拡大率を上昇
            countDownText.transform.localScale = new Vector3(
                countDownText.transform.localScale.x + (deltaTime * 24.0f),
                countDownText.transform.localScale.y + (deltaTime * 24.0f),
                countDownText.transform.localScale.z
                );

            // テキストを移動
            countDownText.transform.Translate(new Vector3(0.0f, deltaTime * 600.0f, 0.0f));


            // カウントダウンが終了した場合の処理
            // コルーチンを使用し、0.5秒後にカウントダウンのテキストを非表示にする
            StartCoroutine(EndCountDoun());
        }

    }

    // カウントダウン処理
    void UpdateCountDown()
    {
        // カウントが最大値と同値の場合(カウント開始時や減少時)
        // カウントUI(またはログ)の表示を行う
        if (currenOneSecond == oneSecond)
        {
            // カウントダウンのテキストを有効化
            countDownText.gameObject.SetActive(true);
            // テキストの拡大率をリセット
            countDownText.transform.localScale = Vector3.one;
            // テキストの回転をリセット
            countDownText.transform.rotation = Quaternion.identity;

            // (ToDo)UIの表示を行い、更新を有効にする
            // カウントダウンの残り秒数の値に合わせて表示を更新
            countDownText.text = countDownTime.ToString();

            // デバッグ用のログ出力
            Debug.Log("カウントダウン: " + countDownTime + "秒");
        }

        // テキストの拡大率を減少
        countDownText.transform.localScale = new Vector3(
            countDownText.transform.localScale.x - (deltaTime * 1.0f),
            countDownText.transform.localScale.y - (deltaTime * 1.0f),
            countDownText.transform.localScale.z
            );
        // テキストを回転
        countDownText.transform.Rotate(new Vector3(0.0f, 0.0f, deltaTime * 360.0f));

        // 経過時間を更新
        currenOneSecond -= deltaTime;

        // 1秒経過したらカウントダウンを進める
        if (currenOneSecond > 0.0f)
        {
            // 経過していない場合は早期リターン
            return;
        }

        // カウントダウン時間を減らす
        countDownTime--;
        // 次の1秒の時間を設定
        currenOneSecond = oneSecond;


        // カウントダウンが終了した場合
        if (countDownTime <= 0)
        {
            // カウントダウンのテキストを更新
            countDownText.text = "GO!";
            // テキストの拡大率をリセット
            countDownText.transform.localScale = Vector3.one;
            // テキストの回転をリセット
            countDownText.transform.rotation = Quaternion.identity;

            // デバッグ用のログ出力
            Debug.Log("行きましょう");

            // カウントダウンを無効にする
            isStartCountDown = false;
            // カウントダウンが終了したフラグを立てる
            endCountDown = true;
            // ゲーム開始の処理を行う
            gameManagerScript.StartGame();
        }


    }

    IEnumerator EndCountDoun()
    {

        // カウント終了後の後処理
        // 0.5秒後にカウントダウンのテキストを非表示にする
        yield return new WaitForSeconds(0.5f);
        // カウントダウンのテキストを無効化
        countDownText.gameObject.SetActive(false);


    }
}
