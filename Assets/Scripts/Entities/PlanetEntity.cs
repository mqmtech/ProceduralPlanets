using UnityEngine;
using System.Collections;

public class PlanetEntity : MonoBehaviour 
{
	[SerializeField]
	SphericalMover _mover;

	PlanetGenerator _planet;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		_planet = FindClosestPlanet();
		_mover.Init(transform, _planet);
	}

	PlanetGenerator FindClosestPlanet()
	{
		return FindObjectOfType<PlanetGenerator>();
	}

	void Update()
	{
		bool isOk = _mover.Move(Vector3.forward);
		if(!isOk)
		{
			Debug.Log("Can Not Move :(");
		}
	}
}
