using UnityEngine;
using System.Collections;

public class SinusNoiseGenerator : INoise3DGenerator
{

	public float Generate(Vector3 i)
	{
		return (((Mathf.Sin(i.x) * Mathf.Cos(i.y) * Mathf.Sin(i.z))+1f)*0.5f);
	}
}
