/*  PlanetAtmospehre Shader
	SPACE for UNITY - Space Scene Construction Kit
	https://www.imphenzia.com/space-for-unity

	Shader to render static stars (custom skybox) without tinting or noise nebulas.

	Version History
	1.5		- New version of the atmopshere shader for more solid atmospehres.
*/

// Modified: Shader from Unity wiki

Shader "SpaceUnity/PlanetAtmosphere"
{
    Properties
    {
        //_Color("Color", Color) = (0, 0, 0, 1)
        _AtmoColor("Atmosphere Color", Color) = (0.5, 0.5, 1.0, 1)
        _Size("Size", Float) = 0.1
        _Falloff("Falloff", Float) = 5
        _Transparency("Transparency", Float) = 15       
    }
   
	SubShader
    {  
		Tags {
            	"LightMode" = "Always"            	
	    		"Queue" = "Transparent+1"
        		"RenderType" = "Transparent"
		}
 		Pass {
            Name "AtmosphereBase"

            Cull Front
            //Blend SrcAlpha One
			Blend One One
           
            CGPROGRAM
				#pragma exclude_renderers gles
                #pragma vertex vert
                #pragma fragment frag
               
                #pragma fragmentoption ARB_fog_exp2
                #pragma fragmentoption ARB_precision_hint_fastest
               
                #include "UnityCG.cginc"
               
                //uniform float4 _Color;
                uniform float4 _AtmoColor;
                uniform float _Size;
                uniform float _Falloff;
                uniform float _Transparency;
               
                struct v2f {
                    float4 pos : SV_POSITION;
                    float3 normal : TEXCOORD0;
                    float3 worldvertpos : TEXCOORD1;
                };

                v2f vert(appdata_base v) {
                    v2f o;
                   
                    v.vertex.xyz += v.normal*_Size;
                    o.pos = UnityObjectToClipPos (v.vertex);
                    o.normal = v.normal;
                    o.worldvertpos = mul(unity_ObjectToWorld, v.vertex);
                   
                    return o;
                }
              
                float4 frag(v2f i) : COLOR {
                    i.normal = normalize(i.normal);
                    float3 viewdir = normalize(i.worldvertpos-_WorldSpaceCameraPos);                   
                    float4 color = _AtmoColor;
					float f = pow(saturate(dot(viewdir, i.normal)), _Falloff);
					f = pow(f, 2.6 - (f * 18)); 
					f *= 1.2 * _Transparency*dot(normalize(_WorldSpaceLightPos0), i.normal);
					color.rgb = saturate(color.rgb * f);
					color.rgb *= min(f, 1);
                    return color;
                }
            ENDCG
        }
    }
   
    FallBack "Diffuse"
}