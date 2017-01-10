Shader "Colorify/Real-time/2 colors/Transparent/Bumped Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)
	_Range ("Range", Range (0.0, 2.0)) = 0.01
	_HueRange ("Hue Range", Range (0.0, 4.0)) = 0.1
	_PatCol2 ("Pattern Color 2", Color) = (1,1,1,1)
	_NewColor2 ("New Color 2", Color) = (1,1,1,1)
	_Range2 ("Range 2", Range (0.0, 2.0)) = 0.01
	_HueRange2 ("Hue Range 2", Range (0.0, 4.0)) = 0.1
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 300
	
CGPROGRAM
#pragma surface surf Lambert alpha
#pragma target 3.0

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
fixed4 _PatCol;
fixed4 _NewColor;
half _Range;
half _HueRange;
fixed4 _PatCol2;
fixed4 _NewColor2;
half _Range2;
half _HueRange2;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	half hue = atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001);
	half targetHue = atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001);
	half targetHue2 = atan2(1.73205 * (_PatCol2.g - _PatCol2.b), 2 * _PatCol2.r - _PatCol2.g - _PatCol2.b + 0.001);	
	o.Albedo = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),
	                     sqrt(saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))
						      * saturate(1.0 - min(abs(hue-targetHue),6.28319 - abs(hue-targetHue))/(_HueRange * _HueRange)))),
				   (_NewColor2.rgb - _PatCol2.rgb + c.rgb),
				    sqrt(saturate(1.0 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range2 * _Range2))
				         * saturate(1.0 - min(abs(hue-targetHue2),6.28319 - abs(hue-targetHue2))/(_HueRange2 * _HueRange2))));
	o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Transparent/Diffuse"
}