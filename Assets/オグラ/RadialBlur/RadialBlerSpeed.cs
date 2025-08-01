using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialBlerSpeed : MonoBehaviour
{
    private PlayerOperation playerOperation;
    public GameObject playerOperationScript;

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

    //加速中のブラー強度とサンプル数の倍率
    public float accelerationBlurMultiplier = 2.0f;

    //ブラー中心
    public Vector2 fixedBlurCenter = new Vector2(0.5f, 0.6f);

    // Start is called before the first frame update
    void Start()
    {
        playerOperation = playerOperationScript.GetComponent<PlayerOperation>();

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
            //ゲーム開始時、ShaderCameraを一旦無効にしておく
            shaderCamera.enabled = false;

            //ブラーの中心を固定値に設定
            shaderCamera.blurCenter = fixedBlurCenter;
        }
    }

    // Update is called once per frame
    void Update()
    {
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

            //加速中かを判断しブラーの最大値を一時的に変更
            float currentMaxBlurStrength = maxBlurStrength;
            int currentMaxBlurSamples = maxBlurSamples;

            //if (playerOperation.IsAccelerating)
            //{
            //    currentMaxBlurStrength *= accelerationBlurMultiplier;
            //    currentMaxBlurSamples = Mathf.RoundToInt(maxBlurSamples * accelerationBlurMultiplier);
            //}

            ////速度を0.0から1.0の範囲に正規化
            //float normalizedSpeed = Mathf.Clamp01(currentSpeed / playerOperation.maxSpeed);
            ////ぼかしの強度を速度に応じて線形補間し、ShaderCameraに設定
            //shaderCamera.blurStrength = Mathf.Lerp(minBlurStrength, currentMaxBlurStrength, normalizedSpeed);
            ////サンプル数を速度に応じて線形補間し、整数に丸めてShaderCameraに設定
            //shaderCamera.blurSamples = Mathf.RoundToInt(Mathf.Lerp(minBlurSamples, currentMaxBlurSamples, normalizedSpeed));
        }
    }
}