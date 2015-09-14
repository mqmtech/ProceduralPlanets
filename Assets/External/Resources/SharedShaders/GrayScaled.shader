Shader "Custom/GrayScaled" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_ColorFactor ("Color Factor", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf NoLighting noforwardadd noshadow

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
		half _ColorFactor;
		
		fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
	     {
	         fixed4 c;
	         c.rgb = s.Albedo; 
	         c.a = s.Alpha;
	         return c;
	     }

		void surf (Input IN, inout SurfaceOutput o) 
		{
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed3 luminance = Luminance(c.rgb);	
			o.Albedo = lerp(luminance, c.rgb, _ColorFactor);
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
