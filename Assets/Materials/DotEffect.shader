Shader "PostEffect/DotEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag (v2f i) : SV_Target
            {
                float blockSize = (sin(_Time.y * 0.2) + sin(_Time.y * 0.17)) * 0.5 * 20 + 30;
                
                fixed2 uv = i.uv;
                fixed2 center = fixed2(0.5,0.5) + fixed2(sin(_Time.y * 0.1) ,cos(_Time.y * 0.14)) * 0.25;
                uv -= center;
                fixed2 mosaicUV = floor(uv * _ScreenParams.xy / blockSize) * blockSize / _ScreenParams.xy;
                fixed2 fractUV = frac(uv * _ScreenParams.xy / blockSize);
                mosaicUV += center;
                fixed4 col = tex2D(_MainTex, mosaicUV);
                float ballCol = length(fractUV - fixed2(0.5, 0.5));
                return ballCol < 0.3 * col.r ? col : fixed4(0,0,0,0);
            }
            ENDCG
        }
    }
}

