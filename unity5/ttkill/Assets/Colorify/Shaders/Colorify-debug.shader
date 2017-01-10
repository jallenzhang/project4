// Debug shader for Colorify.
// Debug modes:
// 0 - behaves as simple diffuse
// 1 - shows range mask, red for color 1, green for color 2
// 2 - shows hue mask, red for color 1, green for color 2
// 3 - shows combined mask, red for color 1, green for color 2
Shader "Colorify/Debug" {
Properties {
	_Debug ("Debug mode", Float) = 0.0
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB)", 2D) = "white" {}	
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
	Tags { "RenderType"="Opaque" }
	LOD 200

CGPROGRAM
#pragma surface surf Lambert
#pragma target 3.0

sampler2D _MainTex;
fixed4 _Color;
fixed4 _PatCol;
fixed4 _NewColor;
half _Debug;
half _Range;
half _HueRange;
fixed4 _PatCol2;
fixed4 _NewColor2;
half _Range2;
half _HueRange2;


struct Input {
	float2 uv_MainTex;	
};

void surf (Input IN, inout SurfaceOutput o) {	
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;	
	half hue = atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001);
	half targetHue = atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001);
	half targetHue2 = atan2(1.73205 * (_PatCol2.g - _PatCol2.b), 2 * _PatCol2.r - _PatCol2.g - _PatCol2.b + 0.001);	
	
	half coef1 = saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range));
	half hueCoef1 = saturate(1.0 - min(abs(hue-targetHue),6.28319 - abs(hue-targetHue))/(_HueRange * _HueRange));
	
	half coef2 = saturate(1.0 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range2 * _Range2));
	half hueCoef2 = saturate(1.0 - min(abs(hue-targetHue2),6.28319 - abs(hue-targetHue2))/(_HueRange2 * _HueRange2));
	
	fixed4 debugc;
	
	if (ceil(_Debug) == 0.0)
	{
		o.Albedo = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),sqrt(coef1 * hueCoef1)),(_NewColor2.rgb - _PatCol2.rgb + c.rgb),sqrt(coef2 * hueCoef2));
	}
		
	if (ceil(_Debug) == 1.0)
	{
		debugc.r = coef1;
		debugc.g = coef2;
		debugc.b = 0.0;
		o.Albedo = debugc.rgb;
	}
	
	if (ceil(_Debug) == 2.0)
	{
		debugc.r = hueCoef1;
		debugc.g = hueCoef2;
		debugc.b = 0.0;
		o.Albedo = debugc.rgb;
	}
	
	if (ceil(_Debug) == 3.0)
	{
		debugc.r = sqrt(coef1 * hueCoef1);
		debugc.g = sqrt(coef2 * hueCoef2);
		debugc.b = 0.0;
		o.Albedo = debugc.rgb;
	}

	o.Alpha = c.a;	
}
ENDCG  
}

FallBack "VertexLit"
}
