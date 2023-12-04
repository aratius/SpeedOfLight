Shader "Unlit/BallMetaballModified"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor ("MainColor", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha 
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Calculate view direction
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
                // Calculate the dot product
                float ndotv = max(0, dot(viewDir, i.worldNormal));
                // Interpolate between white and black based on the dot product
                // fixed4 col = lerp(fixed4(1, 0, 0, 1), fixed4(0, 0, 1, 1), ndotv);
                fixed4 col = _MainColor;
                col.a = ndotv;
                return col;
            }
            ENDCG
        }
    }
}
