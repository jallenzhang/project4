// Simplified Bumped Specular shader. Differences from regular Bumped Specular one:
// - no Main Color nor Specular Color
// - specular lighting directions are approximated per vertex
// - writes zero to alpha channel
// - Normalmap uses Tiling/Offset of the Base texture
// - no Deferred Lighting support
// - no Lightmap support
// - fully supports only 1 directional light. Other lights can affect it, but it will be per-vertex/SH.

Shader "Colorify/Real-time/Mobile/2 Colors/Bumped Specular" {
Properties {
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)
	_Range ("Range", Range (0.0, 2.0)) = 0.01
	_PatCol2 ("Pattern Color 2", Color) = (1,1,1,1)
	_NewColor2 ("New Color 2", Color) = (1,1,1,1)
	_Range2 ("Range 2", Range (0.0, 2.0)) = 0.01		
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 250
	
CGPROGRAM
#pragma surface surf MobileBlinnPhong exclude_path:prepass nolightmap noforwardadd halfasview

inline fixed4 LightingMobileBlinnPhong (SurfaceOutput s, fixed3 lightDir, fixed3 halfDir, fixed atten)
{
	fixed diff = max (0, dot (s.Normal, lightDir));
	fixed nh = max (0, dot (s.Normal, halfDir));
	fixed spec = pow (nh, s.Specular*128) * s.Gloss;
	
	fixed4 c;
	c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten*2);
	c.a = 0.0;
	return c;
}

sampler2D _MainTex;
sampler2D _BumpMap;
half _Shininess;
fixed4 _PatCol;
fixed4 _NewColor;
half _Range;
fixed4 _PatCol2;
fixed4 _NewColor2;
half _Range2;


struct Input {
	float2 uv_MainTex;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),
	                     saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))),
					(_NewColor2.rgb - _PatCol2.rgb + c.rgb),
					saturate(1 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range2 * _Range2)));o.Gloss = c.a;
	o.Alpha = c.a;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_MainTex));
}
ENDCG
}

FallBack "Mobile/VertexLit"
}
