Shader "SubsurfaceScattering" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
		_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
		_TransMap ("Translucency Map",2D) = "white" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_TransDistortion ("Tranlucency Distortion",Range(0,0.5)) = 0.1
		_TransPower("Tranlucency Power",Range(1.0,15.0)) = 4.0
		_TransScale("Translucency Scale",Range(0.0,10.0)) = 2.0
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
CGPROGRAM
#pragma surface surf TransBlinnPhong

sampler2D _MainTex;
sampler2D _TransMap;
sampler2D _BumpMap;
float4 _Color;
float _Shininess;
float _TransDistortion;
float _TransPower;
float _TransScale;

struct Input
{
	float2 uv_MainTex;
	float2 uv_BumpMap;
};

struct TransSurfaceOutput
{
	fixed3 Albedo;
	fixed3 Normal;
	fixed3 Emission;
	half Specular;
	fixed Gloss;
	fixed Alpha;
	fixed3 TransCol;
};

inline fixed4 LightingTransBlinnPhong (TransSurfaceOutput s, fixed3 lightDir, half3 viewDir, fixed atten)
{	
	half atten2 = (atten * 2);

	fixed3 diffCol;
	fixed3 specCol;
	float spec;	
	
	half NL = max(0.0h,dot (s.Normal, lightDir));

	half3 h = normalize (lightDir + viewDir);
	
	float nh = max (0, dot (s.Normal, h));
	spec = pow (nh, s.Specular*128.0) * s.Gloss;
	
	diffCol = (s.Albedo * _LightColor0.rgb * NL) * atten2;
	specCol = (_LightColor0.rgb * _SpecColor.rgb * spec) * atten2;

	half3 transLight = lightDir + s.Normal * _TransDistortion;
	float VinvL = saturate(dot(viewDir, -transLight));
	float transDot = pow(VinvL,_TransPower) * _TransScale;
	half3 lightAtten = _LightColor0.rgb * atten2;
	half3 transComponent = lightAtten * (transDot + _Color.rgb) * s.TransCol;
	diffCol += s.Albedo * transComponent;
	
	fixed4 c;
	c.rgb = diffCol + specCol;
	c.a = s.Alpha + _LightColor0.a * _SpecColor.a * spec * atten;
	return c;
}

 void surf (Input IN, inout TransSurfaceOutput o)
 {
	half4 tex = tex2D(_MainTex, IN.uv_MainTex);
	o.Albedo = tex.rgb * _Color.rgb;
	o.Gloss = tex.a;
	o.Alpha = tex.a * _Color.a;
	o.Specular = _Shininess;
	o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
	o.TransCol = tex2D(_TransMap,IN.uv_MainTex).rgb;
}
ENDCG
}

FallBack "Specular"
}