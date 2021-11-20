Shader "Custom/ShieldShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 1
        [HDR]
        _FrontColor ("Front Color", Color) = (1,1,1,1)
        [HDR]
        _BackColor ("Back Color", Color) = (1,1,1,1)
        _ReduceMainCol("Reduce Main Color", Range(0, 10)) = 2
        [HDR]
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelPower("Fresnel Power", Range(0, 10)) = 2
        _VertexAmount ("Vertex Amount", Range(0, 1)) = .1
        _VertexFrequency("Vertex Frequency", Float) = 1
        [HDR]
        _EdgeColor ("Edge Color", Color) = (1,1,1,1)
        _EdgeHighlight("Edge Highlight", Range(0, 10)) = 1
        _EdgeThreshold("Edge Threshold", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "IgnoreProjector"="True" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            Cull Back

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

            float _Intensity;
            float4 _FrontColor;
            float4 _BackColor;
            float _ReduceMainCol;
            float4 _FresnelColor;
            float _FresnelPower;
            float _VertexAmount;
            float _VertexFrequency;
            float4 _EdgeColor;
            float _EdgeHighlight;
            float _EdgeThreshold;

            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 vertex = v.vertex;
                float4 amount = sin(_Time.x * _VertexFrequency * random(v.normal)) + 1;
                vertex += float4(v.normal, 1) * _VertexAmount * amount;
                o.vertex = UnityObjectToClipPos(vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float NdotV = dot(normal, viewDir);

                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _FrontColor;

                float intensity = saturate((NdotV + 1) / 2);

                float4 fresnelDot = 1 - NdotV;
                float4 fresnel = _FresnelColor * fresnelDot * _FresnelPower;

                float edge = col.a > _EdgeThreshold ? 1 : 0;

                return _Intensity * (col * intensity * pow(NdotV, _ReduceMainCol) + (edge * _EdgeHighlight * _EdgeColor) + fresnel);
            }
            ENDCG
        }
        
        Pass
        {
            Cull Front

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            float _Intensity;
            float4 _FrontColor;
            float4 _BackColor;
            float _VertexAmount;
            float _VertexFrequency;
            float4 _EdgeColor;
            float _EdgeHighlight;
            float _EdgeThreshold;

            float random (float2 uv)
            {
                return frac(sin(dot(uv,float2(12.9898,78.233)))*43758.5453123);
            }

            v2f vert (appdata v)
            {
                v2f o;

                float4 vertex = v.vertex;
                float4 amount = sin(_Time.x * _VertexFrequency * random(v.normal)) + 1;
                vertex += float4(v.normal, 1) * _VertexAmount * amount;
                o.vertex = UnityObjectToClipPos(vertex);
                
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _BackColor;

                float3 normal = normalize(i.worldNormal);
                float3 viewDir = normalize(i.viewDir);
                float NdotV = dot(normal, viewDir);
                float intensity = saturate((NdotV + 1.2) / 2);
                
                float edge = col.a > _EdgeThreshold ? 1 : 0;
                
                return _Intensity * (col * intensity + (edge * _EdgeHighlight * _EdgeColor));
            }
            ENDCG
        }
    }
}
