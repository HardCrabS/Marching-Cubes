Shader "Custom/ToonTerrain"
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
					float3 viewDir : TEXCOORD2;
					float3 wpos : TEXCOORD3;
					//Creates a variable that contains fog coordinates. The parameter must be a free TEXCOORD, for example 1 if TEXCOORD1 is free.
					UNITY_FOG_COORDS(1)
				};

				float boundsY;
				float normalOffsetWeight;
				sampler2D ramp;

				sampler2D moistureTexture;
				float4 moistureTexture_ST;

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
					float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
					o.wpos = worldPos;
					//Compute fog amount from clip space position.
					UNITY_TRANSFER_FOG(o, o.vertex);
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
					float specularIntensitySmooth = smoothstep(0.001, 0.2, specularIntensity);
					float4 specular = specularIntensity * _SpecularColor;

					float4 rimDot = 1 - dot(viewDir, normal);

					float h = smoothstep(-boundsY / 2, boundsY / 2, i.wpos.y + i.worldNormal.y * normalOffsetWeight);
					float4 tex = tex2D(ramp, float2(h, .5)).rgba;

					float4 color = tex * (_AmbientColor + light + specular + rimDot);

					//Apply fog (additive pass are automatically handled)
					UNITY_APPLY_FOG(i.fogCoord, color);

					//to handle custom fog color another option would have been 
					//#ifdef UNITY_PASS_FORWARDADD
					//  UNITY_APPLY_FOG_COLOR(i.fogCoord, color, float4(0,0,0,0));
					//#else
					//  fixed4 myCustomColor = fixed4(0,0,1,0);
					//  UNITY_APPLY_FOG_COLOR(i.fogCoord, color, myCustomColor);
					//#endif

					//float4 moisture = tex2D(_MainTex, i.uv);

					//return moisture;
					return color;
				}
				ENDCG
			}
		}
}
