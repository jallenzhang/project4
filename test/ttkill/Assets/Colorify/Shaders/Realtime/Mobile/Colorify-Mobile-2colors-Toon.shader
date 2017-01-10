Shader "Colorify/Real-time/Mobile/2 Colors/Toon Outline" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bump ("Bump", 2D) = "bump" {}
		_ColorMerge ("Color Merge", Range(0.1,20000)) = 8
		_Ramp ("Ramp Texture", 2D) = "white" {}
		_OutlineColor ("Outline color", Color) = (0,0,0,0)
		_Outline ("Outline", Range(0, 0.15)) = 0.08
		_PatCol ("Pattern Color", Color) = (1,1,1,1)
		_NewColor ("New Color", Color) = (1,1,1,1)
		_Range ("Range", Range (0.0, 2.0)) = 0.01	
		_NewColor2 ("New Color 2", Color) = (1,1,1,1)
		_Range2 ("Range 2", Range (0.0, 2.0)) = 0.01		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		Pass {

			Cull Front
			Lighting Off
			ZWrite On
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float3 tangent : TANGENT;
			}; 

			struct v2f
			{
				float4 pos : POSITION;
			};

			float _Outline;
			fixed4 _OutlineColor;

			v2f vert (a2v v)
			{
				v2f o;
				float4 pos = mul( UNITY_MATRIX_MV, v.vertex); 
				float3 normal = mul( (float3x3)UNITY_MATRIX_IT_MV, v.normal);  
				normal.z = -0.4;
				pos = pos + float4(normalize(normal),0) * _Outline;
				o.pos = mul(UNITY_MATRIX_P, pos);

				return o;
			}

			float4 frag (v2f IN) : COLOR
			{
				return _OutlineColor;
			}

			ENDCG

		}

		Pass {

			Cull Back 
			Lighting On
			Tags { "LightMode"="ForwardBase" }

			CGPROGRAM
// Upgrade NOTE: excluded shader from DX11 and Xbox360; has structs without semantics (struct v2f members lightDirection)
			#pragma exclude_renderers d3d11 xbox360
			#pragma vertex vert
			#pragma fragment frag			

			#include "UnityCG.cginc"
			uniform float4 _LightColor0;

			sampler2D _MainTex;
			sampler2D _Bump;
			sampler2D _Ramp;

			float4 _MainTex_ST;
			float4 _Bump_ST;
			
			fixed4 _Color;
			
			fixed4 _PatCol;
			fixed4 _NewColor;
			half _Range;			

			fixed4 _PatCol2;
			fixed4 _NewColor2;
			half _Range2;
			
			float _Tooniness;
			float _ColorMerge;

			struct a2v
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 tangent : TANGENT;

			}; 

			struct v2f
			{
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
				float3 lightDirection;

			};

			v2f vert (a2v v)
			{
				v2f o;
				//Create a rotation matrix for tangent space
				TANGENT_SPACE_ROTATION; 
				//Store the light's direction in tangent space
				o.lightDirection = mul(rotation, ObjSpaceLightDir(v.vertex));
				//Transform the vertex to projection space
				o.pos = mul( UNITY_MATRIX_MVP, v.vertex); 
				//Get the UV coordinates
				o.uv = TRANSFORM_TEX (v.texcoord, _MainTex);  
				o.uv2 = TRANSFORM_TEX (v.texcoord, _Bump);
				return o;
			}

			float4 frag(v2f i) : COLOR  
			{ 
				//Get the color of the pixel from the texture
				float4 c = tex2D (_MainTex, i.uv) * _Color;
				
				c.rgb = lerp(lerp(c.rgb,(_NewColor.rgb - _PatCol.rgb + c.rgb),
	                     saturate(1 - ((c.r - _PatCol.r)*(c.r - _PatCol.r) + (c.g - _PatCol.g)*(c.g - _PatCol.g) + (c.b - _PatCol.b)*(c.b - _PatCol.b)) / (_Range * _Range))),
					(_NewColor2.rgb - _PatCol2.rgb + c.rgb),
					saturate(1 - ((c.r - _PatCol2.r)*(c.r - _PatCol2.r) + (c.g - _PatCol2.g)*(c.g - _PatCol2.g) + (c.b - _PatCol2.b)*(c.b - _PatCol2.b)) / (_Range2 * _Range2)));
				
				//Merge the colours
				c.rgb = (floor(c.rgb*_ColorMerge)/_ColorMerge);

				//Get the normal from the bump map
				float3 n =  UnpackNormal(tex2D (_Bump, i.uv2)); 

				//Based on the ambient light
				float3 lightColor = UNITY_LIGHTMODEL_AMBIENT.xyz;

				//Work out this distance of the light
				float lengthSq = dot(i.lightDirection, i.lightDirection);
				//Fix the attenuation based on the distance
				float atten = 1.0 / (1.0 + lengthSq);
				//Angle to the light
				float diff = saturate (dot (n, normalize(i.lightDirection)));  
				//Perform our toon light mapping 
				diff = tex2D(_Ramp, float2(diff, 0.5));
				//Update the colour
				lightColor += _LightColor0.rgb * (diff * atten); 
				//Product the final color
				c.rgb = lightColor * c.rgb * 2;
				return c; 

			} 

			ENDCG
		}

	}
	FallBack "Diffuse"
}