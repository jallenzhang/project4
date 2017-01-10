Shader "Colorify/Real-time/1 color/Bumped Specular" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)
	_Range ("Range", Range (0.0, 2.0)) = 0.01
	_HueRange ("Hue Range", Range (0.0, 4.0)) = 0.1	
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0


sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
half _Shininess;
fixed4 _PatCol;
fixed4 _NewColor;
half _Range;
half _HueRange;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color.a;
	half hue = atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001);
	half targetHue = atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001);	
	o.Albedo = lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),
	                sqrt(saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))
				         * saturate(1.0 - min(abs(hue-targetHue),6.28319 - abs(hue-targetHue))/(_HueRange * _HueRange))));	
	o.Gloss = c.a / _Color.a;
	o.Alpha = c.a;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Specular"
}
