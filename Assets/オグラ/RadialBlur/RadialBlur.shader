Shader "Unlit/RadialBlur"
{
   Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 入力テクスチャ (カメラのRenderTextureがここに設定される)
        _BlurStrength ("Blur Strength", Range(0.0, 1.0)) = 0.5 // ぼかしの強度
        _BlurSamples ("Blur Samples", Range(1, 64)) = 16 // サンプル数
        _BlurCenter ("Blur Center", Vector) = (0.5, 0.5, 0, 0) // ぼかしの中心 (UV座標)
    }
    SubShader
    {
        // ポストプロセスエフェクトとして使用するため、描画設定を調整
        Tags { "RenderType"="Opaque" "Queue"="Transparent" } // ポストプロセスは描画キューの最後の方で処理されることが多い
        Blend Off // 通常はブレンドなし (上書き)
        ZWrite Off // 深度バッファへの書き込みを無効 (画面全体に描画するため)
        Cull Off // カリングを無効 (裏面も描画)
        ZTest Always // 深度テストを常に通過 (他のオブジェクトの上に描画)

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // Unityのヘルパー関数や構造体を含む

            // アプリケーションからバーテックスシェーダーへの入力データ
            struct appdata
            {
                float4 vertex : POSITION; // 頂点座標
                float2 uv : TEXCOORD0;    // UV座標
            };

            // バーテックスシェーダーからフラグメントシェーダーへの出力データ
            struct v2f
            {
                float2 uv : TEXCOORD0;      // UV座標をそのまま渡す
                float4 vertex : SV_POSITION; // クリップ空間での頂点座標
            };

            // シェーダーのプロパティに対応する変数
            sampler2D _MainTex;
            float _BlurStrength;
            float _BlurSamples;
            float4 _BlurCenter; // float4 で宣言しているが、xyのみ使用

            // バーテックスシェーダー
            v2f vert (appdata v)
            {
                v2f o;
                // 頂点座標をクリップ空間に変換 (画面全体を覆う四角形が描画される)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // UV座標をそのまま渡す (フルスクリーンクアッドの0-1範囲のUV)
                o.uv = v.uv;
                return o;
            }

            // フラグメントシェーダー
            fixed4 frag (v2f i) : SV_Target
            {
                // 現在のピクセルの元の色を取得
                fixed4 col = tex2D(_MainTex, i.uv);

                // ぼかしの中心から現在のUV座標へのベクトル
                // _BlurCenter は UV 座標 (0.0 - 1.0) で与えられる
                float2 blurDir = i.uv - _BlurCenter.xy;

                // ぼかしの強度に応じてベクトルをスケール
                // この値が大きいほど、より遠くのピクセルをサンプリングする
                blurDir *= _BlurStrength;

                fixed4 finalColor = col; // 最終的な色（最初の色は現在のピクセルの色）

                // サンプリングループ
                // _BlurSamples 回、中心に向かってずらした位置をサンプリングし、色を加算
                for (int j = 1; j <= (int)_BlurSamples; j++) // _BlurSamples は int にキャスト
                {
                    // 中心に向かって徐々にずらしたUV座標を計算
                    // (float)j / _BlurSamples は 0 から 1 までの正規化された距離
                    float2 offsetUV = i.uv - blurDir * ((float)j / _BlurSamples);

                    // サンプリングして色を加算
                    finalColor += tex2D(_MainTex, offsetUV);
                }

                // 全てのサンプルの合計をサンプル数で割って平均化
                // 最初の current pixel とループ内の _BlurSamples 回のサンプルを加算したので、合計で _BlurSamples + 1 で割る
                finalColor /= (_BlurSamples + 1);

                return finalColor;
            }
            ENDCG
        }
    }
}