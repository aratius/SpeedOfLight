Shader "PostEffect/MetaballEffect"
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
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // メタボールの効果を計算
                float threshold = 0.9; // 閾値を調整する
                float2 offsets[9] = { float2(-1, -1), float2(0, -1), float2(1, -1), float2(-1, 0), float2(0, 0), float2(1, 0), float2(-1, 1), float2(0, 1), float2(1, 1) };
                float blurSize = 0.005; // ブラーの大きさを調整する
                float sum = 0.0;

                // 周囲のピクセルをサンプリングして蓄積
                for (int j = 0; j < 9; j++)
                {
                    float2 offset = offsets[j] * blurSize;
                    fixed4 c = tex2D(_MainTex, i.uv + offset);
                    sum += (c.r + c.g + c.b) / 3;
                }

                // 閾値に基づいて色を設定
                // col = sum > threshold ? col : fixed4(0, 0, 0, 1);
                // col = sum > threshold ? fixed4(1,1,1,1) : fixed4(0, 0, 0, 1);
                col = sum > threshold ? fixed4(i.uv.x,i.uv.y,1,1) : fixed4(0, 0, 0, 1);

                return col;
            }
            ENDCG
        }
    }
}

