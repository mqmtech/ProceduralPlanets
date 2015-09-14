Shader "MQMTech/TextureColored" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
	}
	
	SubShader {
	    Pass {
	        Fog { Mode Off }
	        //Lighting { Always }
	        CGPROGRAM

	        #pragma vertex vert
	        #pragma fragment frag
	        
	        sampler2D _MainTex;
	        fixed4 _Color;

	        struct appdata 
	        {
	            float4 vertex : POSITION;
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
	            return o;
	        }
	        
	        fixed4 frag (v2f i) : SV_Target 
	        {
	        	return tex2D(_MainTex, i.uv) * _Color;
	     	}
	        ENDCG
	    }
	}
}