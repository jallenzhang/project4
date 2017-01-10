Shader "Colorify/Mask(baked)/2 colors/Bumped Specular" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ColorifyMaskTex ("Colorify mask (RGB)", 2D) = "black" {}
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)
	_PatCol2 ("Pattern Color 2", Color) = (1,1,1,1)
	_NewColor2 ("New Color 2", Color) = (1,1,1,1)	
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma target 3.0


sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _ColorifyMaskTex; 
fixed4 _Color;
half _Shininess;
fixed4 _PatCol;
fixed4 _NewColor;
fixed4 _PatCol2;
fixed4 _NewColor2;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 mask = tex2D(_ColorifyMaskTex, IN.uv_MainTex); 
	o.Albedo = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),mask.r),(_NewColor2.rgb - _PatCol2.rgb + c.rgb),mask.g);
	o.Gloss = c.a / _Color.a;
	o.Alpha = c.a;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Specular"
}
