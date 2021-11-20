// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Tint ("Tint Color", Color) = (1,1,1,1)
        [HDR]
        _AmbientColor ("Ambient Color", Color) = (0.4,0.4,0.4,1)
        _ShadowBands("Shadow bands", Range(1,10)) = 1
        [HDR]
        _SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
        _Glossiness("Glossiness", Float) = 32
        [HDR]
        _RimColor("Rim Color", Color) = (1,1,1,1)
        _RimAmount("Rim Amount", Range(0, 1)) = 0.716
        _RimThreshold("Rim Threshold", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags {"Queue" = "Geometry" "RenderType" = "Opaque"}

        // pass for the directional light (main light)
        Pass
        {
            Tags 
            { 
                "LightMode" = "ForwardBase"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // Compile multiple versions of this shader depending on lighting settings.
			#pragma multi_compile_fwdbase

            #include "UnityCG.cginc"
            // Files below include macros and functions to assist
			// with lighting and shadows.
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 worldNormal : NORMAL;
                float3 viewDir : TEXCOORD1;
                LIGHTING_COORDS(2, 3)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Tint;
            float4 _AmbientColor;
            float _ShadowBands;
            float _Glossiness;
            float4 _SpecularColor;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.viewDir = WorldSpaceViewDir(v.vertex);

                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 tex = tex2D(_MainTex, i.uv);
                
                // check the angle between normal direction and the light direction
                float3 normal = normalize(i.worldNormal);
                float NdotL = dot(_WorldSpaceLightPos0, normal);

                /* 
                    Main Color 
                */
                float atten = LIGHT_ATTENUATION(i); // Macro to get you the combined shadow & attenuation value.

                // used in the specular calculation
                float lightIntensityRaw = smoothstep(0, 0.01, round(NdotL * _ShadowBands) / _ShadowBands * atten);
                
                // divide light into several bands
                float lightIntensity = round(NdotL * _ShadowBands) / _ShadowBands * atten;
                lightIntensity = saturate(lightIntensity);

                // add the color of the main directional light and the amibient light
                float4 light = lightIntensity * _LightColor0 + (1.0 - lightIntensity) * _AmbientColor;

                /* 
                    Specular Blinn-Phong 
                */
                float3 viewDir = normalize(i.viewDir);

                // H = (L + V) / (|| L + V ||)
                float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
                float NdotH = dot(normal, halfVector);

                // _Glossiness ^ 2 makes it easier to control the size of the specular
                float specularIntensity = pow(NdotH * lightIntensityRaw, _Glossiness * _Glossiness);

                // clamping the fadeout at the edge
                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor * _LightColor0;

                /* 
                    Rim
                */
                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor * _LightColor0;

                return tex * _Tint * (light + specular + rim);
            }
            ENDCG
        }

        // pass for each individual light (additive)
        Pass
        {
            Tags 
            { 
                "LightMode" = "ForwardAdd"
            }
            Blend One One     // Additively blend this pass with the previous one(s). This pass gets run once per pixel light.

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_fwdadd_fullshadows

            #include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
                float3 normal : NORMAL;
                float3 viewDir : TEXCOORD1;
                float3 lightDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Tint;
            float _ShadowBands;
            float _Glossiness;
            float4 _SpecularColor;
            float4 _RimColor;
            float _RimAmount;
            float _RimThreshold;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;        // for some reasons, the world normal gives an incorrect result
                o.viewDir = WorldSpaceViewDir(v.vertex);
                o.lightDir = ObjSpaceLightDir(v.vertex);
                
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 tex = tex2D(_MainTex, i.uv);

                /* 
                    Main Color 
                */
                i.lightDir = normalize(i.lightDir);
                fixed atten = LIGHT_ATTENUATION(i); 

                float3 normal = normalize(i.normal);
                fixed NdotL = dot(normal, i.lightDir);

                float lightIntensityRaw = smoothstep(0, 0.01, round(NdotL * _ShadowBands) / _ShadowBands * atten);
                float lightIntensity = round(NdotL * _ShadowBands) / _ShadowBands * atten;

                float4 light = _LightColor0 * lightIntensity;

                /* 
                    Specular Blinn-Phong 
                */
                float3 viewDir = normalize(i.viewDir);

                float3 halfVector = normalize(i.lightDir + viewDir);
                float NdotH = dot(normal, halfVector);

                float specularIntensity = pow(NdotH * lightIntensityRaw, _Glossiness * _Glossiness);

                float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
                float4 specular = specularIntensitySmooth * _SpecularColor * _LightColor0;

                /* 
                    Rim
                */
                float4 rimDot = 1 - dot(viewDir, normal);
                float rimIntensity = rimDot * pow(NdotL, _RimThreshold);
                rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
                float4 rim = rimIntensity * _RimColor * _LightColor0;

                return tex * _Tint * (light + specular + rim);
            }
            ENDCG
        }
    }
    FallBack "VertexLit"    // Use VertexLit's shadow caster/receiver passes.
}
