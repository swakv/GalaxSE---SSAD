/*  SU_Asteroid shader (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

	Simple custom shader for asteroids.

	Version History
	1.5		- New shader feature in version 1.5.
*/

Shader "SpaceUnity/SU_Asteroid" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
	}

	CGINCLUDE
	sampler2D _MainTex;
	sampler2D _BumpMap;
	fixed4 _Color;
	half _Shininess;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
	};

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = tex.rgb * _Color.rgb;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG

	SubShader {
		Tags{ "RenderType" = "Opaque" "Queue" = "Geometry-100"}
		LOD 400

		CGPROGRAM
		#pragma surface surf BlinnPhong 
		ENDCG
	}

	FallBack "Legacy Shaders/Specular"
}
