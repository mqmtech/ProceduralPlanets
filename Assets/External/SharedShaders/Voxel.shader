Shader "MQMTech/Voxel" {
	Properties {
		//_AmbientColor ("Ambient", Color) = (1,1,1,1)
		_Color ("Up Color", Color) = (1,1,1,1)
		_LateralColor ("Lateral Color", Color) = (1,1,1,1)
		
		_BottomColor ("Bottom Color", Color) = (1,1,1,1)
		_TopColor ("Top Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			fixed4 color;
		};

		fixed4 _Color;
		fixed4 _LateralColor;
		
		fixed4 _BottomColor;
		fixed4 _TopColor;
		
		sampler2D _MainTex;
		
		// This will go to surface function, then the SurfaceOutput will go to the Lighting function then to the finalColor function and then to the screen
		void vert (inout appdata_full v, out Input o) 
		{
        	o.uv_MainTex = v.texcoord;
        	o.worldPos = mul(_Object2World, float4(v.vertex.xyz, 1.0));
        	o.color = v.color;
      	}
		
		// Step1
		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			fixed4 boxColor = clamp(abs(IN.color.r), 0.0, 1.0)*_LateralColor + abs(IN.color.g)*_Color;
			fixed4 c = boxColor;
			
			half2 p = IN.uv_MainTex;
			half d = abs(p.y)-0.5;
			c = lerp(c*0.4, c, smoothstep(0.0, 0.1, d));
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
