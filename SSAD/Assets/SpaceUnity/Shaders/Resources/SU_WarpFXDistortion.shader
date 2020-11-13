/*  SU_WarpFXDistortion shader (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

	Shader that visually warps the environment to simulate warping through space and time.

	It's used by the SU_TravelWarp component and it grabs the screen at RenderQueue "Gemoetry -10" (which is equal to 1990 at the time of writing)
	and applies the warp effect to everything. Anything that has a render queue under 1990 will be affected by the warp render.

	If you want to include objects in the effect, set the render queue to 1989 or lower. Alternatively you could change this shader to  include higher
	render queues and make sure the objects you don't want to be affected by the warp to have a higher rendere queue number.

	Version History
	1.5		- New shader feature in version 1.5
*/

Shader "SpaceUnity/SU_WarpFXDistortion" {
	Properties{
		_NoiseTex("Noise Texture (RG)", 2D) = "white" {}
		_WarpStrength("_WarpStrength", Range(0, 1.0)) = 0
		_WarpBrightness("_WarpBrightness", Range(0, 4.0)) = 1
	}

	Category{
		Tags{ "Queue" = "Geometry-10" }
		SubShader{
			GrabPass{
				Name "BASE"
				Tags{ "LightMode" = "Always" }
			}

			Pass{
				Name "BASE"
				Tags{ "LightMode" = "Always" }
				Fog{ Color(0,0,0,0) }
				Lighting Off
				Cull Off
				ZWrite On
				ZTest LEqual
				Blend SrcAlpha OneMinusSrcAlpha
				AlphaTest Greater 0

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma fragmentoption ARB_fog_exp2
				#include "UnityCG.cginc"

				sampler2D _GrabTexture : register(s0);
				float4 _NoiseTex_ST;
				sampler2D _NoiseTex;
				uniform float _WarpStrength;
				uniform float _WarpRadiusMultiplier;
				uniform float4 _WarpBend;
				uniform float _WarpSpeed;
				uniform float _WarpBrightness;

				struct data {
					float4 vertex : POSITION;
					float3 normal : NORMAL;
					float4 texcoord : TEXCOORD0;
				};

				struct v2f {
					float4 position : POSITION;
					float4 screenPos : TEXCOORD0;
					float2 uvmain : TEXCOORD2;
					float distortion : TEXCOORD3;
					//float4 debug : TEXCOORD4;
				};

				// The vertex shader bends a procedural genertated mesh (created by SU_TravelWarp) based on input to shader parameters
				v2f vert(data i) {
					v2f o;					
					i.vertex += float4(-_WarpBend.z, 0, _WarpBend.x,0) * pow(saturate(abs(i.vertex.y) / 50000), 4) * 20000;
					i.vertex *= float4(max(_WarpRadiusMultiplier,1), 1, max(_WarpRadiusMultiplier,1), 1);
					o.position = UnityObjectToClipPos(i.vertex);
					i.texcoord.y -= _Time.x * _WarpSpeed; 					
					o.uvmain = TRANSFORM_TEX(i.texcoord, _NoiseTex);
					
					o.distortion = 0.2;
					o.screenPos = o.position;
					//o.debug = float4(pow(saturate(abs(i.vertex.y) / 50000), 4), 0, 0, 1);
					return o;
				}

				// The fragment shader uses a noise texture to warp the grabbed screen by different amounts
				half4 frag(v2f i) : COLOR{
					//return i.debug; // Debugging
					//return tex2D(_NoiseTex, i.uvmain)  * _WarpStrength; // Debugging
					float2 screenPos = i.screenPos.xy / i.screenPos.w;   
					screenPos.x = (screenPos.x + 1) * 0.5; 
					screenPos.y = (screenPos.y + 1) * 0.5; 
					half4 offsetColor = tex2D(_NoiseTex, i.uvmain);
					screenPos.x += ((offsetColor.r + offsetColor.r) / 2) * i.distortion * _WarpStrength;
					screenPos.y += ((offsetColor.g + offsetColor.g) / 2) * i.distortion * _WarpStrength;
					screenPos.y = 1 - screenPos.y;
					half4 col = tex2D(_GrabTexture, screenPos);
					col += tex2D(_NoiseTex, i.uvmain)*0.1;					
					return col * _WarpBrightness;
				}
				ENDCG
			}
		}
	}
}