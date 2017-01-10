Shader "Colorify/Mask(baked)/2 colors/Unlit/Texture" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_ColorifyMaskTex ("Colorify mask (RGB)", 2D) = "black" {}	
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)
	_PatCol2 ("Pattern Color 2", Color) = (1,1,1,1)
	_NewColor2 ("New Color 2", Color) = (1,1,1,1)	
}

SubShader {
	Tags { "RenderType"="Opaque" }
	LOD 100
	
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _ColorifyMaskTex;
			float4 _MainTex_ST;
			
			fixed4 _Color;
			fixed4 _PatCol;
			fixed4 _NewColor;
			half _Range;
			half _HueRange;
			fixed4 _PatCol2;
			fixed4 _NewColor2;
			half _Range2;
			half _HueRange2;
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 c = tex2D(_MainTex, i.texcoord) * _Color;
				fixed4 mask = tex2D(_ColorifyMaskTex, i.texcoord);
				c.rgb = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),mask.r),(_NewColor2.rgb - _PatCol2.rgb + c.rgb),mask.g); 
				return c;
			}
		ENDCG
	}
}

}
