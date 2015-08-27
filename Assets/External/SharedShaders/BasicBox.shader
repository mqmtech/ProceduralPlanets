Shader "MQMTech/BasicBox" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		//#pragma surface surf Standard fullforwardshadows
		#pragma surface surf SimpleSpecular fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float3 normal;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		
		void vert (inout appdata_full v, out Input o) 
		{
        	UNITY_INITIALIZE_OUTPUT(Input,o);
        	o.worldPos = mul(_Object2World, float4(v.vertex.xyz, 1.0));
        	o.normal = mul(_Object2World, float4(v.normal.xyz, 1.0));
      	}
		
		half4 LightingSimpleSpecular (SurfaceOutput s, half3 lightDir, half3 viewDir, half atten) 
		{
	        half3 h = normalize (lightDir + viewDir);
	        float spec = pow(max (0, dot (s.Normal, h)), 48.0);
	        half diff = pow(max (0, dot (s.Normal, lightDir)), 10.0);
	        
	        half4 c;
	        c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * spec) * (atten * 2);
	        c.a = s.Alpha;
	        
	        return c;
	    }

		void surf (Input IN, inout SurfaceOutput o) {
			//fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			fixed4 colorByCoreDistance = lerp(fixed4(1.0,0.0,0.0,1.0), fixed4(0.5,1.0,0.5,1.0), smoothstep(1.5, 10.0, length(IN.worldPos)));
			fixed4 color = fixed4(IN.normal, 1.0)*0.25 + colorByCoreDistance*0.75;
			fixed4 c = color * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
