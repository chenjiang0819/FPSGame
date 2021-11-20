Shader "Custom/LaserBeamShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        _MainSpeed ("Main Speed", Vector) = (-0.5,0,0,0) 
        _Mask ("Mask", 2D) = "white" {}
        _Noise("Noise", 2D) = "white" {}
        _NoiseScale("Noise Scale", float) = 0.5
        _NoiseSpeed ("Noise Speed", Vector) = (-1,0,0,0) 
        _NoisePower("Noise Power", float) = 2
        _NoiseAmount("Noise Amount", Range(0,1)) = 0.1
        _DissolveAmount("Dissolve Amount", Range(0,1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            float4 _MainSpeed;
            sampler2D _Mask;
            sampler2D _Noise;
            float _NoiseScale;
            float4 _NoiseSpeed;
            float _NoisePower;
            float _NoiseAmount;
            float _DissolveAmount;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float texOffset = _Time.x * _MainSpeed.xy;

                float2 noiseUV = i.uv + _Time.x * _NoiseSpeed.xy;
                fixed noise = tex2D(_Noise, noiseUV * _NoiseScale);
                noise = pow(noise, _NoisePower);
                float2 uv = lerp(i.uv, noise, _NoiseAmount);

                uv = float2(uv.x + texOffset, uv.y);
                fixed4 col = tex2D(_MainTex, uv);

                fixed4 mask = tex2D(_Mask, i.uv);
                float4 dissolve = lerp(col, noise * col.a, _DissolveAmount);

                col = dissolve * mask * i.color * _Color;

                fixed4 output;
                output.rgb = col.rgb;
                output.a = mask.r * dissolve.a * i.color.a;

                return output;
            }
            ENDCG
        }
    }
}
