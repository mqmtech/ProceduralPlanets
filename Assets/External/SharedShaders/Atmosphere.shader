Shader "MQMTech/Atmosphere" {
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		_HomosphereColor("Homos phereColor", Color) = (1, 1, 1, 1)
		_HeterosphereColor("Heterosphere Color", Color) = (1, 1, 1, 1)
		_AtmospherePowerFactor("Atmosphere Power Factor", Float) = 2.0
	}
	
    SubShader 
    {
        Pass 
        {
			Tags
			{
				"Queue" = "Transparent"
				"RenderType" = "Overlay"
			}
			
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZWrite Off

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            sampler2D _Noise;
            float3 _PlanetPosition;
            
            fixed4 _HomosphereColor;
            fixed4 _HeterosphereColor;
            
            half _AtmospherePowerFactor;

            struct v2f {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
                half2 uvNoise : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
            };

            v2f vert (appdata_full v)
            {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                
                o.worldPos =  mul (_Object2World, float4(v.vertex.xyz, 1.0)).xyz;
                
                o.color = v.color;
                o.uvNoise = v.texcoord;
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
            	half3 planetPosition = half3(_Object2World[0][3], _Object2World[1][3], _Object2World[2][3]);
            	
            	half3 posLocalToPlanet = i.worldPos - planetPosition;
            	half distanceToPlanet = length(posLocalToPlanet);
            	half3 dirLocalToPlanet = posLocalToPlanet / distanceToPlanet;
            	
            	half3 cameraPosLocalToVertex = _WorldSpaceCameraPos - i.worldPos;
            	half3 cameraToVertexDir = normalize(cameraPosLocalToVertex);
            	half3 cameraPosLocalToPlanet = _WorldSpaceCameraPos - planetPosition;
            	
            	float outerSpaceFactor = lerp(0., 1., smoothstep(0., 200., length(cameraPosLocalToPlanet) - distanceToPlanet));
            	
            	// Atmosphere view from outer space
            	half atmosphereFactor = lerp(1.0, 0.1, pow(max(dot(dirLocalToPlanet, cameraToVertexDir), 0.), _AtmospherePowerFactor));
            	fixed4 colorFromOuterSpace = lerp(_HomosphereColor, _HeterosphereColor, atmosphereFactor);
            	atmosphereFactor *= lerp(0.0, 1.0, smoothstep(0.5, 0.70, atmosphereFactor));
            	
            	half smoothToOuterSpaceFactor = lerp(1.0, 0.0, smoothstep(0.7, 1.0, atmosphereFactor));
            	
            	atmosphereFactor = lerp(atmosphereFactor, 1.0, outerSpaceFactor);
            	colorFromOuterSpace = lerp(colorFromOuterSpace, _HeterosphereColor, atmosphereFactor);
            	atmosphereFactor *= smoothToOuterSpaceFactor;
            	colorFromOuterSpace.a *= atmosphereFactor;
            	
            	// sky color
            	fixed4 skyColor = fixed4(1., 1., 1., 0.5);
            	
            	
            	
            	float skyVisibleFactor = lerp(0., 1., smoothstep(0., 10., length(cameraPosLocalToVertex)));
            	skyColor = lerp(fixed4(0., 0., 0., 0.), skyColor, skyVisibleFactor);
            	
            	// Mix atmosphere color and sky color
            	fixed4 finalColor = lerp(skyColor, colorFromOuterSpace, smoothstep(-10., 4., length(cameraPosLocalToPlanet) - distanceToPlanet));
            	return finalColor;
            }
            ENDCG

        }
    }
}