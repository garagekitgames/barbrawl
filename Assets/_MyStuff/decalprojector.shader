
Shader "Projector/Decal" {
    Properties {      
		_Color("Main Color", Color) = (1,1,1,1)
        _Decal ("Cookie", 2D) = "" {}    
		_Slider("Slider", Range(0,1)) = 0.0
		[Toggle(UV2)] _UV2("Use Second UV Map?", Float) = 0
    }
 
    Subshader {
        Tags {"Queue"="Transparent"}
        Pass {
            ZWrite Off
			Cull Off
            ColorMask RGBA
            Blend SrcAlpha OneMinusSrcAlpha
            Offset -1, -1
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag  
			#pragma shader_feature UV2
            #include "UnityCG.cginc"
         
            struct v2f {
                float4 uvShadow : TEXCOORD0;
                float4 pos : SV_POSITION;
            };
         
            float4x4 unity_Projector;
            float4x4 unity_ProjectorClip;
			float _Slider;
			float4 _Color;
         
            v2f vert (appdata_full v)
            {
                v2f o;
				float x = v.texcoord.x;
				float y = v.texcoord.y;
				
				#if UV2
				x = v.texcoord2.x;
				y = v.texcoord2.y;				
				#endif
				x= x * 2 -1;			
				y= y * -2 +1;
				
				float4 vertexUV = float4(x , y, 0, 1);
			    float4 normalview = UnityObjectToClipPos(v.vertex);
			    o.pos = lerp(vertexUV, normalview, _Slider);
                o.uvShadow = mul (unity_Projector, v.vertex);                          
                return o;
            }          
         
            sampler2D _Decal;          
         
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texS = tex2Dproj (_Decal, UNITY_PROJ_COORD(i.uvShadow)) * _Color;                                                  
                return texS;
            }
            ENDCG
        }
    }
}