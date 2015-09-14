using UnityEngine;
using System.Collections;

[System.Serializable]
public class SphericalMover
{
	[SerializeField]
	float _speed = 1f;

	//[SerializeField]
	float _jumpSpeed = 25f;

	Transform _host;
	PlanetGenerator _planet;
	SphericalPhysics _physics;

	public void Init(Transform host, PlanetGenerator planet, SphericalPhysics physics)
	{
		_host = host;
		_planet = planet;
		_physics = physics;
	}

	public void Move(Vector3 velocity)
	{
		_physics.AddVelocity(velocity);
	}

	public void Jump()
	{
		Vector3 normal = _host.transform.position - _planet.transform.position;
		normal.Normalize();

		_physics.AddVelocity(normal * _jumpSpeed);
	}
}
