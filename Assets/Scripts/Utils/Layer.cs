using UnityEngine;
using System.Collections;

public static class Layer 
{
	public static int Terrain
	{
		get
		{
			return LayerMask.NameToLayer("Terrain");
		}
	}
}
