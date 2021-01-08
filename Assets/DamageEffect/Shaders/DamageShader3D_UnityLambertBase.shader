Shader "DamageEffect/DamageBlink3D_UnityShadows"
{
	Properties
	{
		_MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		_Highlight ("Highlight", Color) = (0,0,0,1)		
		_TimeScale ("Time Scale", Range(0,10)) = 5
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM		
		#pragma surface surf Lambert noforwardadd
			
			sampler2D _MainTex;
			fixed4 _Color;
			fixed4 _Highlight;
			float _TimeScale;

			struct Input {
				float2 uv_MainTex;
			};
			
			void surf (Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
				
				c = c * 1.5;
				c = c + c;
				float t = cos(_Time.y * 15 * _TimeScale);
				c.rgb = 1 - (t + 1) * 0.5 + t * c.rgb;
				c.rgb *= _Color;
				c.rgb += _Highlight;
				
				c.rgb *= c.a;		
				
				o.Albedo = c.rgb;
				o.Alpha = c.a;
			}
		ENDCG
		
	}
	FallBack "Self-Illumin/VertexLit"
}
