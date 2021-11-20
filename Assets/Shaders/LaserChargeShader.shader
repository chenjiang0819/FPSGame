// Shader "Custom/ChargeShellShader"
// {
//     Properties
//     {
//         _MainTex ("Texture", 2D) = "white" {}
//         [HDR]
//         _FrontColor ("Front Color", Color) = (1,1,1,1)
//         _Threshold ("Threshold", Float) = 1
//     }
//     SubShader
//     {
//         Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent"}
//         LOD 100
//         Blend SrcAlpha OneMinusSrcAlpha
//         ZWrite Off
//         Cull Back

//         Pass
//         {
//             CGPROGRAM
//             #pragma vertex vert alpha
//             #pragma fragment frag alpha

//             #include "UnityCG.cginc"

//             struct appdata
//             {
//                 float4 vertex : POSITION;
//                 float2 uv : TEXCOORD0;
//                 float3 normal : NORMAL;
//             };

//             struct v2f
//             {
//                 float2 uv : TEXCOORD0;
//                 float4 vertex : SV_POSITION;
//                 float3 normal : NORMAL;
//             };

//             sampler2D _MainTex;
//             float4 _MainTex_ST;

//             float4 _FrontColor;
//             float _Threshold;

//             v2f vert (appdata v)
//             {
//                 v2f o;
//                 o.vertex = UnityObjectToClipPos(v.vertex);
//                 o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                 o.normal = v.normal;
//                 return o;
//             }

//             fixed4 frag (v2f i) : SV_Target
//             {
//                 float NdotU = saturate(dot(i.normal, float3(0, 0, 1)));

//                 fixed4 col = tex2D(_MainTex, i.uv);
//                 col *= _FrontColor;

//                 return col * pow(NdotU, _Threshold);
//             }
//             ENDCG
//         }
//     }
// }
Shader "Custom/ChargeShellShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR]
        _FrontColor ("Front Color", Color) = (1,1,1,1)
        _Threshold ("Threshold", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent"}
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Back

        Pass
        {
            CGPROGRAM
            #pragma vertex vert alpha
            #pragma fragment frag alpha

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _FrontColor;
            float _Threshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float NdotV = saturate(dot(normal, viewDir));

                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _FrontColor;

                return col * pow(NdotV, _Threshold);
            }
            ENDCG
        }
    }
}
