using UnityEngine;
using System.Collections;

public class SinusNoiseGenerator : INoise3DGenerator
{

	public float Generate(Vector3 i)
	{
		i.y *= 0.5f;


		i *= 2.15f;

		float n = 0f;
		n += 0.50f * (((Mathf.Sin(i.x) * Mathf.Cos(i.y) * Mathf.Sin(i.z))+1f)*0.5f); i += Vector3.one * 1265495.336f; i*= 2.1543f;
		n += 0.25f * (((Mathf.Cos(i.x) * Mathf.Sin(i.y) * Mathf.Cos(i.z))+1f)*0.5f); i*= 3.14566f;
		n += 0.125f * (((Mathf.Sin(i.x) * Mathf.Cos(i.y) * Mathf.Sin(i.z))+1f)*0.5f);
		n /= 0.875f;

		return n;
	}
}
