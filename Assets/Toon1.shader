// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/Toon1"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"
				#include "Lighting.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float3 normal : NORMAL;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					float3 worldNormal : NORMAL;
					float3 viewDir : TEXCOORD1;
					float3 wpos : TEXCOORD2;
				};

				float boundsY;
				float normalOffsetWeight;
				sampler2D ramp;

				sampler2D _MainTex;
				fixed4 _Color;
				float4 _AmbientColor;
				float _Glossiness;
				float4 _SpecularColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.worldNormal = UnityObjectToWorldNormal(v.normal);
					o.viewDir = WorldSpaceViewDir(v.vertex);
					float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
					o.wpos = worldPos;
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 normal = normalize(i.worldNormal);
					float NdotL = dot(_WorldSpaceLightPos0, normal);

					float lightIntensity = smoothstep(0, 0.3, NdotL);
					float4 light = lightIntensity * _LightColor0;

					float3 viewDir = normalize(i.viewDir);

					float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
					float NdotH = dot(normal, halfVector);

					float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
					float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
					float4 specular = specularIntensitySmooth * _SpecularColor;

					float4 rimDot = 1 - dot(viewDir, normal);

					float h = smoothstep(-boundsY / 2, boundsY / 2, i.wpos.y + i.worldNormal.y * normalOffsetWeight);
					float4 tex = tex2D(ramp, float2(h, .5)).rgba;

					return tex * (_AmbientColor + light + specularIntensity + rimDot);
				}
				ENDCG
			}
		}
}
