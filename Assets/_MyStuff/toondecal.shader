Shader "Toon/Lit Decal Effect" {
    Properties {
        _Color ("Main Color", Color) = (0.5,0.5,0.5,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}	
		_DecalTex("Decal (RGBA)", 2D) = "black" {}
		_EmissionTex("Emission (RGBA)", 2D) = "black" {}
        _Ramp ("Toon Ramp (RGB)", 2D) = "black" {}
    }

    SubShader {
        Tags { "RenderType"="Transparent" }
        
		Blend One OneMinusSrcAlpha
		ColorMask RGBA
        CGPROGRAM
        #pragma surface surf ToonRamp keepalpha

        sampler2D _Ramp;

        // custom lighting function that uses a texture ramp based
        // on angle between light direction and normal
        #pragma lighting ToonRamp exclude_path:prepass
        inline half4 LightingToonRamp (SurfaceOutput s, half3 lightDir, half atten) {
            #ifndef USING_DIRECTIONAL_LIGHT
            lightDir = normalize(lightDir);
            #endif

            half d = dot (s.Normal, lightDir)*0.5 + 0.5;
            half3 ramp = tex2D (_Ramp, float2(d,d)).rgb;

            half4 c;
            c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
            c.a = s.Alpha;
            return c;
        }

        sampler2D _MainTex, _DecalTex, _EmissionTex;
        float4 _Color;

        struct Input {
            float2 uv_MainTex : TEXCOORD0;
			float2 uv2_DecalTex : TEXCOORD1;
        };

        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			float4 d = tex2D(_DecalTex, IN.uv2_DecalTex);
			float4 e = tex2D(_EmissionTex, IN.uv2_DecalTex);	
            o.Albedo = (c.rgb * (1-d.a))+ (d * 2);
			o.Emission = (e * 10);			
            o.Alpha = c.a ;
        }

        ENDCG
    }

    Fallback "Diffuse"
}