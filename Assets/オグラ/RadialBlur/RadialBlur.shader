Shader "Unlit/RadialBlur"
{
   Properties
    {
        //入力テクスチャ
        _MainTex ("Texture", 2D) = "white" {}
        //ぼかしの強度
        _BlurStrength ("Blur Strength", Range(0.0, 1.0)) = 0.5
        //サンプル数
        _BlurSamples ("Blur Samples", Range(1, 64)) = 16
        //ぼかしの中心 (UV座標)
        _BlurCenter ("Blur Center", Vector) = (0.5, 0.5, 0, 0)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent" }
        Blend Off
        ZWrite Off
        Cull Off
        ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            //バーテックスシェーダーからフラグメントシェーダーへの出力データ
            struct v2f
            {
                //UV座標をそのまま渡す
                float2 uv : TEXCOORD0;
                //クリップ空間での頂点座標
                float4 vertex : SV_POSITION;
            };

            //シェーダーのプロパティに対応する変数
            sampler2D _MainTex;
            float _BlurStrength;
            float _BlurSamples;
            // float4 で宣言しているがxyのみ使用
            float4 _BlurCenter;

            //バーテックスシェーダー
            v2f vert (appdata v)
            {
                v2f o;
                //頂点座標をクリップ空間に変換
                o.vertex = UnityObjectToClipPos(v.vertex);
                //UV座標をそのまま渡す
                o.uv = v.uv;
                return o;
            }

            //フラグメントシェーダー
            fixed4 frag (v2f i) : SV_Target
            {
                //現在のピクセルの元の色を取得
                fixed4 col = tex2D(_MainTex, i.uv);

                //ぼかしの中心から現在のUV座標へのベクトル
                float2 blurDir = i.uv - _BlurCenter.xy;

                //ぼかしの強度に応じてベクトルをスケール
                blurDir *= _BlurStrength;

                //最終的な色（最初の色は現在のピクセルの色）
                fixed4 finalColor = col;

                //サンプリングループ
                //_BlurSamples回中心に向かってずらした位置をサンプリングし色を加算
                for (int j = 1; j <= (int)_BlurSamples; j++)
                {
                    //中心に向かって徐々にずらしたUV座標を計算
                    float2 offsetUV = i.uv - blurDir * ((float)j / _BlurSamples);

                    //サンプリングして色を加算
                    finalColor += tex2D(_MainTex, offsetUV);
                }

                //全てのサンプルの合計をサンプル数で割って平均化
                finalColor /= (_BlurSamples + 1);

                return finalColor;
            }
            ENDCG
        }
    }
}