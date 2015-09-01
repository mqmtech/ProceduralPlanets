using UnityEngine;
using System.Collections;

public class EntityGravity : MonoBehaviour 
{
	[SerializeField]
	float _gravityFactor = 1f;

	// Hack
	PlanetGenerator _planet;

	Transform _host;

	void Awake()
	{
		_host = transform;
	}

	void LateUpdate()
	{
		// Find closest planet
		_planet = FindClosestPlanet();

		// Get gravity...
		Vector3 gravity = _planet.GetGravity(transform.position);

		// Check we're not under the planet
		BaseBox box = _planet.GetVoxelInPosition(transform.position -gravity.normalized*0.25f);
		Vector3 voxelLocalPositon = box.LocalPosition;
		float voxelMag = voxelLocalPositon.magnitude;

		Vector3 myLocalPosition = _planet.ConvertToLocalPosition(_host.position);
		float myMag = myLocalPosition.magnitude;

		float lengthDiff = myMag - voxelMag;

		if(lengthDiff < 0f)
		{
			// I'm inside the planet!
			myLocalPosition = myLocalPosition.normalized * voxelMag;
			_host.position = _planet.ConvertToWorldPosition(myLocalPosition);
		}
		else
		{
			float gravAbsMagnitude = Mathf.Abs(gravity.magnitude*_gravityFactor) * Time.deltaTime;

			float gravityMag = Mathf.Min(lengthDiff, gravAbsMagnitude);
			_host.position += gravity.normalized * gravityMag;
		}
	}

	PlanetGenerator FindClosestPlanet()
	{
		if(_planet == null)
		{
			_planet = FindObjectOfType<PlanetGenerator>();
		}
		return _planet;
	}
}
