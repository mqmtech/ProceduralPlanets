Shader "MQMTech/DefaultUnit" 
{
	Properties
	{
		_ColorMask("Color Mask", 2D) = "white" {}
		_LightMask("_Light Mask", 2D) = "white" {}
		
		_Color0("Color 0", Color) = (1, 1, 1, 1)
		_Color1("Color 1", Color) = (1, 1, 1, 1)
		_Color2("Color 2", Color) = (1, 1, 1, 1)
		_Color3("Color 3", Color) = (1, 1, 1, 1)
		_Color4("Color 4", Color) = (1, 1, 1, 1)
		_Color5("Color 5", Color) = (1, 1, 1, 1)
		
		_Color6("Color 6", Color) = (1, 1, 1, 1)
		_Color7("Color 7", Color) = (1, 1, 1, 1)
		
		_CartoonFactor("Cartoon Factor", Float) = 0.0
	}
	
	SubShader {
	    Pass {
	        Fog { Mode Off }
	        //Lighting { Always }
	        CGPROGRAM

	        #pragma vertex vert
	        #pragma fragment frag
	        
	        sampler2D _ColorMask;
	        sampler2D _LightMask;
	        
	        fixed4 _Color0;
	        fixed4 _Color1;
	        fixed4 _Color2;
	        fixed4 _Color3;
	        fixed4 _Color4;
	        fixed4 _Color5;
	        fixed4 _Color6;
	        fixed4 _Color7;
	        
	        half _CartoonFactor;

	        struct appdata 
	        {
	            float4 vertex : POSITION;
	            //float3 normal : NORMAL;
	            //float4 tangent : TANGENT;
	            float2 texcoord : TEXCOORD;
	        };

	        struct v2f {
	            float4 pos : SV_POSITION;
	            float4 color : COLOR;
	            float2 uv : TEXCOORD;
	        };
	        
	        v2f vert (appdata v) {
	            v2f o;
	            o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
	            o.uv = v.texcoord.xy;
	            // calculate binormal
	            //float3 binormal = cross( v.normal, v.tangent.xyz ) * v.tangent.w;
	            //o.color.xyz = binormal * 0.5 + 0.5;
	            //o.color.w = 1.0;
	            return o;
	        }
	        
	        fixed4 frag (v2f i) : SV_Target 
	        {
	        	fixed4 cMask = tex2D(_ColorMask, i.uv);
	        	fixed4 lMask = tex2D(_LightMask, i.uv);
	        	
	        	fixed4 color = fixed4(0., 0., 0., 0.);
	        	if(cMask.a <= 0.16) 
	        	{
	        	 	color = lerp(_Color0, _Color1, cMask.r);
	        	}
	        	else if(cMask.a <= 0.82)
	        	{
	        	 	color = lerp(_Color2, _Color3, cMask.g);
	    	 	}
	    	 	else
	    	 	{
	    	 		color = lerp(_Color4, _Color5, cMask.b);
	    	 	}
	    	 	
	    	 	color = lerp(color, _Color6, lMask.r);
	    	 	color = lerp(color, _Color7, lMask.g);
	        
	         	return color;
	     	}
	        ENDCG
	    }
	}
}