
// シェーダーコマンド
// 
Shader "Hidden/ShaderTest"
{
    Properties
    {
        // メインテクスチャ
        _MainTex ("Texture", 2D) = "white" {}
        // 中心座標
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        // ブラーの幅
        _BlurWidth ("Blur Width", Float) = 0.01
        // ブラーのサンプル数
        _NumSamples ("Num Samples", Float) = 10
       
    }
    SubShader
    {
       Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // not used, but often available
            float4 _Center;     // xy: 中心座標
            float _BlurWidth;   // ブラーの幅
            float _NumSamples;  // サンプル数（整数扱い）

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 direction = i.uv - _Center.xy;

                float3 color = float3(0, 0, 0);
                for (int sampleIndex = 0; sampleIndex < (int)_NumSamples; ++sampleIndex)
                {
                    float2 offset = direction * (_BlurWidth * sampleIndex);
                    float2 sampleUV = i.uv + offset;
                    color += tex2D(_MainTex, sampleUV).rgb;
                }

                color *= (1.0 / _NumSamples);
                return float4(color, 1.0);
            }

            ENDCG
        }
    }
}
