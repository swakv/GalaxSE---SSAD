/*  SU_StaticStars shader
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

	Shader to render static stars (custom skybox) without tinting or noise nebulas.

	Initial version
*/

Shader "SpaceUnity/Planet Rings" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	ZWrite On
	LOD 200
	Blend SrcAlpha OneMinusSrcAlpha
	CGPROGRAM
	#pragma surface surf Lambert addshadow keepalpha
	
	sampler2D _MainTex;
	fixed4 _Color;
	
	struct Input {
		float2 uv_MainTex;
	};
	
	void surf (Input IN, inout SurfaceOutput o) {
		fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
		o.Albedo = c.rgb;
		o.Alpha = c.a;
	}
	ENDCG
}

Fallback "Transparent/VertexLit"
}
