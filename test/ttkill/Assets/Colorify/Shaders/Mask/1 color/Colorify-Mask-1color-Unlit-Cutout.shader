Shader "Colorify/Mask(baked)/1 color/Unlit/Cutout" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)	
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	_ColorifyMaskTex ("Colorify mask (RGB)", 2D) = "black" {}	
	_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5	
	_PatCol ("Pattern Color", Color) = (1,1,1,1)
	_NewColor ("New Color", Color) = (1,1,1,1)	
}

SubShader {
	Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
	LOD 100
	
	Lighting Off
	
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
			fixed _Cutoff;
			
			fixed4 _Color;
			fixed4 _PatCol;
			fixed4 _NewColor;

			
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
				clip(c.a - _Cutoff);
				fixed4 mask = tex2D(_ColorifyMaskTex, i.texcoord);
				c.rgb = lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),mask.r);
				return c;
			}
		ENDCG
	}
}

}
