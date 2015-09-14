using UnityEngine;
using System.Collections;

public class PlanetEntity : MonoBehaviour 
{
	[SerializeField]
	SphericalMover _mover;

	[SerializeField]
	SphericalPhysics _physics;

	PlanetGenerator _planet;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		_planet = FindClosestPlanet();
		_mover.Init(transform, _planet, _physics);
	}

	PlanetGenerator FindClosestPlanet()
	{
		return FindObjectOfType<PlanetGenerator>();
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.W))
		{
			_mover.Move(transform.forward);
		}
		if(Input.GetKey(KeyCode.S))
		{
			_mover.Move(-transform.forward);
		}
		if(Input.GetKey(KeyCode.A))
		{
			Quaternion delta = Quaternion.AngleAxis(-64f*Time.deltaTime, transform.up);
			transform.rotation = delta * transform.rotation; 
		}
		if(Input.GetKey(KeyCode.D))
		{
			Quaternion delta = Quaternion.AngleAxis(+64f*Time.deltaTime, transform.up);
			transform.rotation = delta * transform.rotation; 
		}
		if(Input.GetKey(KeyCode.Space))
		{
			if(_physics.IsInFloor())
			{
				_mover.Jump();
			}
		}
	}
}
