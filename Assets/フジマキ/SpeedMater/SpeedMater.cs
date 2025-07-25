using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMater : MonoBehaviour
{
    private PlayerOperation playerOperation;
    public GameObject playerOperationScript;

    private GameManager gameManager;
    public GameObject gamemanagerScript;

    public GameObject speedMater_BackImage;
    public GameObject tacoMeterImage;

    // 最小・最大角度と速度
    private float currentAngle = -120f;
    private const float minAngle = 90f;
    private const float maxAngle = -145f;
    private const float maxSpeed = 3f;

    //ShaderCameraコンポーネントへの参照
    private ShaderCamera shaderCamera;
    //カメラオブジェクトの参照
    public Camera mainCamera;

    //速度0でのブラー強度
    public float minBlurStrength = 0.0f;
    //最高速度でのブラー強度
    public float maxBlurStrength = 0.06f;
    //速度0でのブラーサンプル数
    public int minBlurSamples = 0;
    //最高速度でのブラーサンプル数
    public int maxBlurSamples = 7;

    //ブラー中心
    public Vector2 fixedBlurCenter = new Vector2(0.5f, 0.6f); 

    // Start is called before the first frame update
    void Start()
    {
        playerOperation = playerOperationScript.GetComponent<PlayerOperation>(); // 修正: PlayerOperation コンポーネントを取得
        gameManager = gamemanagerScript.GetComponent<GameManager>(); // 修正: GameManager コンポーネントを取得
        //スピードメーターUI初期化
        speedMater_BackImage.SetActive(false);
        // タコメーター非表示
        tacoMeterImage.SetActive(false);

        //カメラからShaderCameraコンポーネントを取得
        if (mainCamera != null)
        {
            shaderCamera = mainCamera.GetComponent<ShaderCamera>();
        }

        // ShaderCameraが見つからない場合のエラーログ
        if (shaderCamera == null)
        {
            Debug.LogError("SpeedMater: ShaderCameraコンポーネントがメインカメラに見つかりません。メインカメラにShaderCameraスクリプトをアタッチしているか確認してください。");
        }
        else
        {
            // ゲーム開始時、ShaderCameraを一旦無効にしておく（ブラー消すため）
            shaderCamera.enabled = false;

            //ブラーの中心を固定値に設定
            shaderCamera.blurCenter = fixedBlurCenter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        SpeedMaterActive(); // スピードメーター/タコメーターの表示・非表示を更新
        TacoMeterMove(); // タコメーターの動きを更新

        //ShaderCameraとPlayerOperationが正しく取得できていればブラー制御を行う
        if (shaderCamera != null && playerOperation != null)
        {
            //プレイヤーの現在の速度の絶対値を取得
            float currentSpeed = Mathf.Abs(playerOperation.GetPlayerSpeed());

            //ブラーの有効/無効にするロジック
            //速度が0.1fより大きくあればブラーを有効にする
            if (currentSpeed > 0.1f && !shaderCamera.enabled)
            {
                shaderCamera.enabled = true;
            }
            //速度が0.1f以下になればブラーを無効にする
            else if (currentSpeed <= 0.1f && shaderCamera.enabled)
            {
                shaderCamera.enabled = false;
            }

            //速度を0.0から1.0の範囲に正規化
            float normalizedSpeed = Mathf.Clamp01(currentSpeed / playerOperation.maxSpeed);

            //ぼかしの強度を速度に応じて線形補間し、ShaderCameraに設定
            shaderCamera.blurStrength = Mathf.Lerp(minBlurStrength, maxBlurStrength, normalizedSpeed);

            //サンプル数を速度に応じて線形補間し、整数に丸めてShaderCameraに設定
            shaderCamera.blurSamples = Mathf.RoundToInt(Mathf.Lerp(minBlurSamples, maxBlurSamples, normalizedSpeed));
        }
    }

    void SpeedMaterActive()
    {
        if (gameManager.IsGameStarted())
        {
            speedMater_BackImage.SetActive(true); // スピードメーター表示
            // タコメーターの表示
            tacoMeterImage.SetActive(true); // タコメーター表示
        }
        else
        {
            speedMater_BackImage.SetActive(false); // スピードメーター非表示
            tacoMeterImage.SetActive(false); // タコメーター非表示
        }
    }

    void TacoMeterMove()
    {
        float speed = playerOperation.GetPlayerSpeed();
        float normalized = Mathf.Clamp01(speed / maxSpeed);

        float targetAngle = Mathf.Lerp(minAngle, maxAngle, normalized);
        currentAngle = Mathf.Lerp(currentAngle, targetAngle, Time.deltaTime * 5f);

        tacoMeterImage.transform.localRotation = Quaternion.Euler(0f, 0f, currentAngle);
    }
}
