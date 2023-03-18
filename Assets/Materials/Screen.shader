Shader "Custom/Screen"
{

    Properties
    {
        _BufferTex("", 2DArray) = "" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            // Cull off ZTest Always
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #define RESOLUTION 256

            UNITY_DECLARE_TEX2DARRAY(_BufferTex);
            uint _Frame;

            struct appdata
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 projectorSpacePos : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float2 distance : TEXCOORD3;
            };

            sampler2D _ProjectorTexture;
            float4x4 _ProjectorMatrixVP;
            float4 _ProjectorPos;
            float _DMin;
            float _DMax;

            //スリットスキャン結果の過去のテクスチャを返却
            float3 GetHistory(float2 uv, uint offset)
            {
                uint i = (_Frame + RESOLUTION - offset) & (RESOLUTION - 1);
                // uv.y = lerp(uv.y, 1 - uv.y, _VFlip);
                return UNITY_SAMPLE_TEX2DARRAY(_BufferTex, float3(frac(uv * 3), i)).rgb;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.projectorSpacePos = mul(mul(_ProjectorMatrixVP, unity_ObjectToWorld), v.vertex);
                o.projectorSpacePos = ComputeScreenPos(o.projectorSpacePos);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.distance = fixed2(length(_ProjectorPos.xyz - mul(unity_ObjectToWorld, v.vertex)), 0.);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // half4 color = 1;
                i.projectorSpacePos.xyz /= i.projectorSpacePos.w;
                float2 uv = i.projectorSpacePos.xy;

                // NOTE: エリア内で正規化したデプステクスチャ
                float dNormalized = (1 - 0) / (_DMax - _DMin) * (i.distance.x - _DMin) + 0;
                // return fixed4(fixed3(dNormalized, dNormalized, dNormalized), 1.);
                float delay = dNormalized * (RESOLUTION - 2);
                uint offset = (uint)delay;
                float3 p1 = GetHistory(uv, offset + 0);
                float3 p2 = GetHistory(uv, offset + 1);
                float4 color = float4(lerp(p1, p2, frac(delay)), 1);

                // カメラの範囲外には適用しない
                fixed3 isOut = step((i.projectorSpacePos - 0.5) * sign(i.projectorSpacePos), 0.5);
                float alpha = isOut.x * isOut.y * isOut.z;
                // プロジェクターから見て裏側の面には適用しない
                alpha *= step(-dot(lerp(-_ProjectorPos.xyz, _ProjectorPos.xyz - i.worldPos, _ProjectorPos.w), i.worldNormal), 0);

                return color * alpha;
            }
            ENDCG
        }
    }
}