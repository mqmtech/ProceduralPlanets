using UnityEngine;
using System.Collections;

[System.Serializable]
public class SphericalMover
{
	[SerializeField]
	float _speed = 1f;

	Transform _host;
	PlanetGenerator _planet;

	public void Init(Transform host, PlanetGenerator planet)
	{
		_host = host;
		_planet = planet;

		SetupCoordinates();
	}

	public void SetupCoordinates()
	{
		Quaternion rotation;
		_planet.GetOrientation(_host, out rotation);
		_host.rotation = rotation;
	}

	public void Move(Vector3 direction)
	{
		Vector3 stepMagnitude = direction * _speed * Time.deltaTime;
		_host.position = _host.position + stepMagnitude;

		SetupCoordinates();
	}
}
