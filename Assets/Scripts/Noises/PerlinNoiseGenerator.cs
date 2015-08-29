using UnityEngine;
using System.Collections;
using MQMTech.Mathematics;

using vec4 = UnityEngine.Vector4;
using vec3 = UnityEngine.Vector3;
using vec2 = UnityEngine.Vector2;

public class PerlinNoiseGenerator : INoise3DGenerator
{
	float hash( float n ) { return mMath.fract(mMath.sin(n)*43758.5453123f); }

	float noise( vec3 x )
	{
		vec3 p = mMath.floor(x);
		vec3 f = mMath.fract(x);
		f = mMath.mul(mMath.mul(f,f), (mMath.vec3(3.0f)-mMath.mul(mMath.vec3(2.0f), f)));
		
		float n = p.x + p.y*157.0f + 113.0f*p.z;
		return mMath.mix(mMath.mix(mMath.mix( hash(n + 0.0f), hash(n+  1.0f),f.x),
               mMath.mix(hash(n+157.0f), hash(n+158.0f),f.x),f.y),
               mMath.mix(mMath.mix( hash(n+113.0f), hash(n+114.0f),f.x),
  	           mMath.mix(hash(n+270.0f), hash(n+271.0f),f.x),f.y),f.z);
	}

	float fbm(vec3 p)
	{
		float f = 0.0f;
		
		f += 0.500000f * noise(p); p *= 2.02f;
		f += 0.250000f * noise(p); p *= 2.04f;
		f += 0.125000f * noise(p); p *= 2.06f;
		f += 0.062500f * noise(p); p *= 2.08f;
		f += 0.031250f * noise(p); p *= 3.03f;
		f += 0.015625f * noise(p);
		f /= 0.984375f;
		
		return f;
	}

	public float Generate(Vector3 i)
	{
		return fbm(i);
	}
}
