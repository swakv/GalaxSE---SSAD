/*  Space Scene Construction Kit Editor C# Script (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

	DESCRIPTION:
	Shader used to render stars and noise (texture must tile in any direction) to a custom skybox.

	Version History
	1.5     - New feature in version 1.5.
*/


Shader "SpaceUnity/SU_StaticStars_Tint_Noise" {
	Properties {
		_MainTex ("Stars Texture (RGB)", 2D) = "black" {}
		_NoiseTex ("Noise Texture (RGB)", 2D) = "black" {}
		_NoiseColor ("Noise Color (RGB)", COLOR) = (0.0,0.1,0.3,1.0)
		_StarsColor ("Stars Color (RGB)", COLOR) = (1.0, 1.0, 1.0, 1.0)
		_StarsIntensity ("Stars Intensity", FLOAT) = 1.0
	}
	SubShader {
		Tags { "Queue"="Background" "RenderType"="Background" }
		Cull Back 
		ZWrite Off
		Fog { Mode Off }    
	    Blend One One

		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
							
			uniform sampler2D _MainTex;
			uniform sampler2D _NoiseTex;
			uniform fixed4 _NoiseColor;
			uniform fixed4 _StarsColor;
			uniform half _StarsIntensity;
	
			struct vertexInput {
				half4 vertex : POSITION;
				fixed4 texcoord: TEXCOORD0;
			};
	
			struct vertexOutput {		
				half4 pos : SV_POSITION;
				fixed4 tex : TEXCOORD0;
			};
			
			vertexOutput vert(vertexInput v) {
				vertexOutput o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.tex = v.texcoord;
				return o;
			}
			
			half4 frag(vertexOutput i) : COLOR {
				half4 texMain = tex2D(_MainTex, i.tex.xy);
				half4 texNoise = tex2D(_NoiseTex, i.tex.xy);
				return (texNoise * _NoiseColor) + (texMain * _StarsColor * _StarsIntensity);
			}
	
			ENDCG
		}
	} 
	FallBack "Diffuse"
}
