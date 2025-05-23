Shader "Custom/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _KernelSize ("Kernel Size", Float) = 5
        _Sigma ("Gaussian Sigma", Float) = 1.0
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // x = 1/width, y = 1/height
            float _KernelSize;
            float _Sigma;

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

            float Gaussian(float x, float y, float sigma)
            {
                float s = sigma * sigma;
                return exp(-(x*x + y*y) / (2.0 * s)) / (6.2831853 * s); // 1 / (2πσ²)
            }

            fixed4 frag(v2f i) : SV_Target
            {
                int halfKernel = int(_KernelSize * 0.5);
                float totalWeight = 0.0;
                float3 color = float3(0, 0, 0);

                for (int x = -halfKernel; x <= halfKernel; ++x)
                {
                    for (int y = -halfKernel; y <= halfKernel; ++y)
                    {
                        float2 offset = float2(x, y) * _MainTex_TexelSize.xy;
                        float weight = Gaussian(x, y, _Sigma);
                        color += tex2D(_MainTex, i.uv + offset).rgb * weight;
                        totalWeight += weight;
                    }
                }

                color /= totalWeight;
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
