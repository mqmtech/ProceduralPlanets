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
		_mover.Move(transform.forward);

		//<- left to right from rotation <-

		if(Input.GetKey(KeyCode.A))
		{
			Quaternion delta = Quaternion.AngleAxis(-64f*Time.deltaTime, transform.up);
			transform.rotation = delta * transform.rotation; 
		}
		else if(Input.GetKey(KeyCode.D))
		{
			Quaternion delta = Quaternion.AngleAxis(+64f*Time.deltaTime, transform.up);
			transform.rotation = delta * transform.rotation; 
		}
	}

	void OnDrawGizmosSelected()
	{
		if(_planet == null)
		{
			return;
		}

		Gizmos.color = Color.green;
		Vector3 pos = transform.position + transform.up*0.5f;
		Vector3 target = pos + transform.forward * 4f;
		Gizmos.DrawLine(pos, target);
	}
}
