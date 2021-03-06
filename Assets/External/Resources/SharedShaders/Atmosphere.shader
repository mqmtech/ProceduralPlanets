﻿Shader "MQMTech/Atmosphere" {
	Properties
	{
		_Noise("Noise", 2D) = "white" {}
		_HomosphereColor("Homos phereColor", Color) = (1, 1, 1, 1)
		_HeterosphereColor("Heterosphere Color", Color) = (1, 1, 1, 1)
		_StarColor("Star Color", Color) = (1, 1, 1, 1)
		_AtmospherePowerFactor("Atmosphere Power Factor", Float) = 2.0
		
		_SkyColor("Sky Color", Color) = (1, 1, 1, 1)
		_CloudsColor("Clouds Color", Color) = (0.5, 0.5, 0.5, 0.5)
		
		_SunColor("Sun Color", Color) = (0.5, 0.5, 0.5, 0.5)
		_SunDir("SunDir", Vector) = (1.0, 1.0, 1.0, 1.0)
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
			Fog {Mode Off}

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Assets/External/Resources/ShaderLibrary/MQMNoise.cginc"
            
            sampler2D _Noise;
            float3 _PlanetPosition;
            
            fixed4 _HomosphereColor;
            fixed4 _HeterosphereColor;
            fixed4 _StarColor;
            half _AtmospherePowerFactor;
            
            fixed4 _SkyColor;
            fixed4 _CloudsColor;
            
            fixed4 _SunColor;
            half4 _SunDir;

            struct v2f {
                float4 pos : SV_POSITION;
                fixed3 color : COLOR0;
                half2 uvNoise : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
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
            	
            	float outerSpaceFactor = lerp(0., 1.0, smoothstep(0., 150., length(cameraPosLocalToPlanet) - distanceToPlanet));
            	
            	// Atmosphere view from outer space
            	half atmosphereFactor = lerp(1.0, 0.1, pow(max(dot(dirLocalToPlanet, cameraToVertexDir), 0.), _AtmospherePowerFactor));
            	atmosphereFactor *= lerp(0.0, 1.0, smoothstep(0.30, 0.70, atmosphereFactor));
            	fixed4 colorFromOuterSpace = lerp(_HomosphereColor, _HeterosphereColor, atmosphereFactor);
            	
            	half smoothToOuterSpaceFactor = lerp(1.0, 0.0, smoothstep(0.3, 1.0, atmosphereFactor));
            	
            	atmosphereFactor = lerp(atmosphereFactor, 1.0, outerSpaceFactor);
            	colorFromOuterSpace = lerp(colorFromOuterSpace, _StarColor, atmosphereFactor);
            	atmosphereFactor *= smoothToOuterSpaceFactor;
            	colorFromOuterSpace.a *= atmosphereFactor;
            	
            	// sky color
            	fixed4 skyColor = _SkyColor;
            	half cloudsFactor = smoothstep(0.2, 1., fbm((i.worldPos+float3(_Time.y, _Time.x, _Time.x))*0.015));
            	skyColor = lerp(skyColor, _CloudsColor, cloudsFactor);
            	
            	// Sun color
            	half3 sunDir = normalize(_SunDir.xyz);
            	half sunPower = max(dot(sunDir, -cameraToVertexDir), 0.);
            	half sunFactor = pow(sunPower, 500.) * 1.;
            	sunFactor 	  += pow(sunPower, 50.) * 0.6;
            	sunFactor 	  += pow(sunPower, 5.) * 0.35;
            	skyColor += _SunColor * clamp(sunFactor, 0., 1.);
            	
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