Shader "Colorify/Mask(baked)/1 color/Transparent/Bumped Diffuse" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
	_ColorifyMaskTex ("Colorify mask (RGB)", 2D) = "black" {}	
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)	
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 300
	
CGPROGRAM
#pragma surface surf Lambert alpha
#pragma target 3.0

sampler2D _MainTex;
sampler2D _BumpMap;
sampler2D _ColorifyMaskTex; 
fixed4 _Color;
fixed4 _PatCol;
fixed4 _NewColor;


struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

void surf (Input IN, inout SurfaceOutput o) {
	fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
	fixed4 mask = tex2D(_ColorifyMaskTex, IN.uv_MainTex); 
	o.Albedo = lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),mask.r);
	o.Alpha = c.a;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
}
ENDCG
}

FallBack "Transparent/Diffuse"
}