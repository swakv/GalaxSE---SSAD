/*  SU_AsteroidFade shader (version: 1.5)
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity
	(c) 2017 Imphenzia AB

	Shader used by asteroids that have a vertex shader that shrinks asteroids between _InnerRadius and _OuterRadius from _AsteroidOrigin.
	_FadeFalloffExp property can be set for a different curve, 1.0 is linear, use 0.5, 0.25, 0.125... or 2, 4, 8 etc for different exponential curves.
	The _AsteroidOrigin and _InnterRadius, _OuterRadius must be set from external scripts. SU_Asteroid does this.

	Version History
	1.5		- New feature in version 1.5.
*/ 
Shader "SpaceUnity/SU_AsteroidFade" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess("Shininess", Range(0.03, 1)) = 0.078125
		_MainTex("Base (RGB) Gloss (A)", 2D) = "white" {}
		_BumpMap("Normalmap", 2D) = "bump" {}
		_InnerRadius("Inner Radius", float) = 20000.0
		_OuterRadius("Outer Radius", float) = 15000.0
		_FadeFalloffExp("Fade Falloff Exponent", float) = 1.0 // 1.0 = Linear
	}

	CGINCLUDE
	sampler2D _MainTex;
	sampler2D _BumpMap;
	fixed4 _Color;
	half _Shininess;
	uniform float4 _AsteroidOrigin;
	uniform float _InnerRadius;
	uniform float _OuterRadius;
	uniform float _FadeFalloffExp;

	struct Input {
		float2 uv_MainTex;
		float2 uv_BumpMap;
		float alpha;
		float4 position;
	};

	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);		
		float4 worldPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
		float falloffRange = _OuterRadius - _InnerRadius;
		v.vertex *= pow(saturate(clamp(_OuterRadius - length(_AsteroidOrigin - worldPos),0, falloffRange)  / falloffRange), _FadeFalloffExp);
		// The below remarked line could be used instead if alpha fading should be enabled too
		//o.alpha = pow(saturate(clamp(_OuterRadius - length(_AsteroidOrigin - worldPos), 0, falloffRange) / falloffRange), _FadeFalloffExp);
	}

	void surf(Input IN, inout SurfaceOutput o) {
		fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
		o.Albedo = tex.rgb * _Color.rgb;
		o.Gloss = tex.a;
		o.Alpha = tex.a * _Color.a;// *IN.alpha;
		o.Specular = _Shininess;
		o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	}
	ENDCG

	SubShader {
		// The below remarked line could be used instead if alpha fading should be enabled too
		//Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"  "DisableBatching" = "True"}
		Tags{ "RenderType" = "Opaque"  "DisableBatching" = "True" "Queue" = "Geometry-100"  }
		LOD 400

		CGPROGRAM
		// The below remarked line could be used instead if alpha fading should be enabled too
		// #pragma surface surf BlinnPhong vertex:vert alpha:fade
		#pragma surface surf BlinnPhong vertex:vert
		#pragma target 3.0
		ENDCG
	}

	FallBack "Legacy Shaders/Specular"
}
