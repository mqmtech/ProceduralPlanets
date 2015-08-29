using UnityEngine;
using System.Collections;

namespace MQMTech.Mathematics
{

	public static class mMath 
	{
		public static float min(float a, float b) { return Mathf.Min(a, b); }
		public static Vector3 min(Vector3 a, Vector3 b) { return new Vector3( Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.z, b.z)); }
		public static Vector2 min(Vector2 a, Vector2 b) { return new Vector2( Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y)); }

		public static float max(float a, float b) { return Mathf.Max(a, b); }
		public static Vector3 max(Vector3 a, Vector3 b) { return new Vector3( Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.z, b.z)); }
		public static Vector2 max(Vector2 a, Vector2 b) { return new Vector2( Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y)); }

		public static float abs(float a) { return Mathf.Abs(a); }
		public static Vector3 abs(Vector3 a) { return new Vector3( Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z)); }
		public static Vector2 abs(Vector2 a) { return new Vector2( Mathf.Abs(a.x), Mathf.Abs(a.y)); }

		public static float sin(float value) { return Mathf.Sin(value); }
		public static Vector3 sin(Vector3 value) { return new Vector3( Mathf.Sin(value.x), Mathf.Sin(value.y), Mathf.Sin(value.z)); }
		public static Vector2 sin(Vector2 value) { return new Vector2( Mathf.Sin(value.x), Mathf.Sin(value.y)); }

		public static float cos(float value) { return Mathf.Cos(value); }
		public static Vector3 cos(Vector3 value) { return new Vector3( Mathf.Cos(value.x), Mathf.Cos(value.y), Mathf.Cos(value.z)); }
		public static Vector2 cos(Vector2 value) { return new Vector2( Mathf.Cos(value.x), Mathf.Cos(value.y)); }

		public static float atan2(float y, float x) { return Mathf.Atan2(x, y); }

		public static float clamp(float value, float min, float max) { return Mathf.Clamp(value, min, max); }
		public static Vector3 clamp(Vector3 value, float min, float max) { return new Vector3( Mathf.Clamp(value.x, min, max), Mathf.Clamp(value.y, min, max), Mathf.Clamp(value.z, min, max)); }
		public static Vector2 clamp(Vector2 value, float min, float max) { return new Vector2( Mathf.Clamp(value.x, min, max), Mathf.Clamp(value.y, min, max)); } 

		public static float ceil(float x) { return Mathf.Ceil(x); }
		public static Vector3 ceil(Vector3 value) { return new Vector3( Mathf.Ceil(value.x), Mathf.Ceil(value.y), Mathf.Ceil(value.z)); }
		public static Vector2 ceil(Vector2 value) { return new Vector2( Mathf.Ceil(value.x), Mathf.Ceil(value.y)); } 

		public static float floor(float x) { return Mathf.Floor(x); }
		public static Vector3 floor(Vector3 value) { return new Vector3( Mathf.Floor(value.x), Mathf.Floor(value.y), Mathf.Floor(value.z)); }
		public static Vector2 floor(Vector2 value) { return new Vector2( Mathf.Floor(value.x), Mathf.Floor(value.y)); } 

		public static float fract(float x) { return x - Mathf.Floor(x); }
		public static Vector3 fract(Vector3 value) { return new Vector3( value.x - Mathf.Floor(value.x), value.y - Mathf.Floor(value.y), value.z - Mathf.Floor(value.z)); }
		public static Vector2 fract(Vector2 value) { return new Vector2( value.x - Mathf.Floor(value.x), value.y - Mathf.Floor(value.y)); }

		public static Vector4 vec4(float x, float y, float z, float w) { return new Vector4(x, y, z, w); }
		public static Vector4 vec4(float value) { return new Vector4(value, value, value, value); }
		public static Vector4 vec4(Vector3 xyz, float w) { return new Vector4(xyz.x, xyz.y, xyz.z, w); }
		public static Vector4 vec4(Vector2 xy, Vector2 zw) { return new Vector4(xy.x, xy.y, zw.x, zw.y); }

		public static Vector3 vec3(float x, float y, float z) { return new Vector3(x, y, z); }
		public static Vector3 vec3(float value) { return new Vector3(value, value, value); }
		public static Vector3 vec3(Vector2 xy, float z) { return new Vector3(xy.x, xy.y, z); }
		public static Vector3 vec3(float x, Vector2 yz) { return new Vector3(x, yz.x, yz.y); }

		public static Color color(int r, int g, int b, int a) { return new Color(r, g, b, a); }
		public static Color color(int r, int g, int b) { return new Color(r, g, b, 255); }
		public static Color color(int value) { return new Color(value, value, value, value); }

		public static Vector2 vec2(float x, float y) { return new Vector2(x, y); }
		public static Vector2 vec2(float value) { return new Vector2(value, value); }

		public static float mix(float a, float b, float factor) { return a*(1.0f-factor) + b*factor; }
		public static Vector3 mix(Vector3 a, Vector3 b, float factor) { return a*(1.0f-factor) + b*factor; }
		public static Vector2 mix(Vector2 a, Vector2 b, float factor) { return a*(1.0f-factor) + b*factor; }

		public static float smoothstep(float min, float max, float value) { return clamp((value-min)/(max-min), 0.0f, 1.0f); }
		public static Vector3 smoothstep(float min, float max, Vector3 value) { return new Vector3(smoothstep(min, max, value.x),smoothstep(min, max, value.y), smoothstep(min, max, value.z) ); }
		public static Vector2 smoothstep(float min, float max, Vector2 value) { return new Vector2(smoothstep(min, max, value.x),smoothstep(min, max, value.y) ); }

		public static float dot(Vector3 a, Vector3 b) { return Vector3.Dot(a, b); }
		public static float dot(Vector2 a, Vector2 b) { return Vector2.Dot(a, b); }

		public static float distance(Vector3 a, Vector3 b) { return Vector3.Distance(a, b);}
		public static float distance(Vector2 a, Vector2 b) { return Vector2.Distance(a, b);}

		public static float length(Vector3 a) { return a.magnitude;}
		public static float length(Vector2 a) { return a.magnitude;}

		public static float length2(Vector3 a) { return a.sqrMagnitude;}
		public static float length2(Vector2 a) { return a.sqrMagnitude;}

		public static float mul(float a, float b) { return a*b; }
		public static Vector3 mul(Vector3 a, Vector3 b) { return new Vector3(a.x*b.x, a.y*b.y, a.z*b.z); }
		public static Vector2 mul(Vector2 a, Vector2 b) { return new Vector2(a.x*b.x, a.y*b.y); }

		public static Matrix4x4 mat4(float m00, float m01, float m02, float m03, float m10, float m11, float m12, float m13, float m20, float m21, float m22, float m23, float m30, float m31, float m32, float m33)
		{
			Matrix4x4 matrix = new Matrix4x4();

			// mColumnRow
			matrix.m00 = m00;
			matrix.m01 = m01;
			matrix.m02 = m02;
			matrix.m03 = m03;

			matrix.m10 = m10;
			matrix.m11 = m11;
			matrix.m12 = m12;
			matrix.m13 = m13;

			matrix.m20 = m20;
			matrix.m21 = m21;
			matrix.m22 = m22;
			matrix.m23 = m23;

			matrix.m30 = m30;
			matrix.m31 = m31;
			matrix.m32 = m32;
			matrix.m33 = m33;

			return matrix;
		}

		public static Vector3 mult3x3(Matrix4x4 matrix, Vector3 vector)
		{
			return matrix.MultiplyVector(vector);
		}

		public static Vector3 mul(Matrix4x4 matrix, Vector3 vector)
		{
			return matrix.MultiplyPoint3x4(vector);
		}

		public static Vector3 mul4x4(Matrix4x4 matrix, Vector3 vector)
		{
			return matrix.MultiplyPoint(vector);
		}
	}
}