Shader "Custom/Fog"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ColorRamp ("Color ramp", 2D) = "white" {}
        _FogAmount ("Fog amount", Range(0, 10)) = 1
        _FogIntensity ("For intensity", Range(0, 1)) = .7
        _AffectSkybox ("Affect Skybox (> 1 yes, < 1 no)", Range(0, 1)) = 0
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
            sampler2D _CameraDepthTexture;
            sampler2D _ColorRamp;
            float _FogAmount;
            float _FogIntensity;
            float _AffectSkybox;

            fixed4 frag (v2f i) : SV_Target
            {
                // Get the original color of this pixel
                fixed4 orCol = tex2D(_MainTex, i.uv);

                // Get the depth value from the camera
                float depth = tex2D(_CameraDepthTexture, i.uv);
                depth = Linear01Depth(depth);

                // Return the color of the skybox
                if (_AffectSkybox < 1 && depth >= _ProjectionParams.z)
                    return orCol;

                // Apply the fog amount factor and clamp the result between 0 and 1
                depth *= _FogAmount;
                depth = saturate(depth);

                // Get the fog color from the color ramp texture
                fixed4 fogCol = tex2D(_ColorRamp, (float2(depth, 0)));

                // Mix the original color and fog color together according to the depth
                return lerp(orCol, fogCol, depth * _FogIntensity);
            }
            ENDCG
        }
    }
}
