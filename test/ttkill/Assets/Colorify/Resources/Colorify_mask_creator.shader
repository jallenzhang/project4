Shader "Hidden/Colorify_mask_creator" 
{
	Properties 
	{		
		_MainTex ("Base (RGB)", 2D) = "white" {}	
	}

	SubShader 
	{
		Pass
		{
		
		CGPROGRAM
		#pragma vertex vert_img
		#pragma fragment frag
		#pragma target 3.0
		
		#include "UnityCG.cginc"
		
		sampler2D _MainTex;		
		fixed4 _Color;
		fixed4 _PatCol;
		float _Range;
		float _HueRange;
		fixed4 _PatCol2;		
		float _Range2;
		float _HueRange2;
 
		
		float4 frag(v2f_img i) : COLOR
		{
			float4 c = tex2D(_MainTex, i.uv) * _Color;
			
			float hue = atan2(1.73205 * (c.g - c.b), 2 * c.r - c.g - c.b + 0.001);
			float targetHue = atan2(1.73205 * (_PatCol.g - _PatCol.b), 2 * _PatCol.r - _PatCol.g - _PatCol.b + 0.001);
			float targetHue2 = atan2(1.73205 * (_PatCol2.g - _PatCol2.b), 2 * _PatCol2.r - _PatCol2.g - _PatCol2.b + 0.001);	 
			
			float coef1 = saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range));
			float hueCoef1 = saturate(1.0 - min(abs(hue-targetHue),6.28319 - abs(hue-targetHue))/(_HueRange * _HueRange));
			float coef2 = saturate(1 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range2 * _Range2));
			float hueCoef2 = saturate(1.0 - min(abs(hue-targetHue2),6.28319 - abs(hue-targetHue2))/(_HueRange2 * _HueRange2));
			
			float brightness = c.r * 0.21 + c.g * 0.72 + c.b * 0.07;
			
			c.r = sqrt(coef1 * hueCoef1);
			c.g = sqrt(coef2 * hueCoef2);
			c.b = brightness;
			c.a = 0;			
			
			return c;		
		}

		ENDCG
		} 
	}
}