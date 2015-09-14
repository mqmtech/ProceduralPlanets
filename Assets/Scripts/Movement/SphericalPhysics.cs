using UnityEngine;
using System.Collections;
using MQMTech.Mathematics;

public class SphericalPhysics : MonoBehaviour 
{
	const float kOutterBias = 4f;

	[SerializeField]
	float _gravityFactor = 1f;

	Collider _collider;

	float _drag = 0.1f;

	Vector3 _currentAcceleration;
	Vector3 _currentVelocity;

	PlanetGenerator _planet;

	Transform _host;

	void Awake()
	{
		_host = transform;
		_collider = GetComponentInChildren<Collider>();
	}

	public void AddVelocity(Vector3 velocity)
	{
		_currentVelocity += velocity;
	}

	public bool IsInFloor()
	{
		Vector3 localPosition = _planet.ConvertToLocalPosition(_host.transform.position);
		Vector3 surfaceLocalPosition = _planet.GetSurfaceLocalPosition(localPosition + localPosition.normalized * 4f);

		return Mathf.Abs(localPosition.sqrMagnitude - surfaceLocalPosition.sqrMagnitude) < 1e-1f;
	}

	void Update()
	{
		_planet = FindClosestPlanet();

		Vector3 localPosition = _planet.ConvertToLocalPosition(_host.position);

		float dt = Time.deltaTime;

		Vector3 deltaPosition = _currentVelocity * dt;
		deltaPosition += _planet.GetGravity(localPosition) * _gravityFactor * dt;

		Vector3 targetLocalPosition = localPosition + deltaPosition;

		//CheckCollisions(_host.transform.position, deltaPosition, ref targetLocalPosition, ref _currentVelocity);

		targetLocalPosition = SetOverTheSuface(targetLocalPosition);

		_host.transform.position = _planet.ConvertToWorldPosition(targetLocalPosition);

		UpdateOrientation();

		ApplyDrag();
	}

	bool CheckCollisions(Vector3 sourceWorldPos, Vector3 deltaPosition, ref Vector3 oTargetPosition, ref Vector3 oVelocity)
	{
		Vector3 velocity = deltaPosition.normalized;
		Vector3 velForwardComponent = Vector3.Dot(_host.transform.forward, velocity) * _host.transform.forward;
		Vector3 velRightcomponent = Vector3.Dot(_host.transform.right, velocity) * _host.transform.right;
		Vector3 velForwardRightComponent = velForwardComponent + velRightcomponent;

		Vector3 normal = sourceWorldPos - _planet.transform.position;
		normal.Normalize();

		Vector3 positionBias = normal * _collider.bounds.extents.x + velForwardRightComponent.normalized *_collider.bounds.extents.x;
		Vector3 rayStartWorldPos = sourceWorldPos + positionBias;
		Ray ray = new Ray(rayStartWorldPos, velForwardRightComponent.normalized);
		Debug.DrawRay(rayStartWorldPos,  velForwardRightComponent.normalized * velForwardRightComponent.magnitude * 1.0f, Color.red);
		
		BaseBox voxel = null;
		RaycastHit hitInfo;

		if(Physics.Raycast(ray, out hitInfo, velForwardRightComponent.magnitude * 1.0f, 1<<Layer.Terrain))
		{
			oTargetPosition = hitInfo.point - velForwardRightComponent.normalized * _collider.bounds.extents.x - positionBias;
			oVelocity = -oVelocity * 0.1f;

			return true;
		}
		else
		{
			return false;
		}
	}

	Vector3 SetOverTheSuface(Vector3 targetLocalPosition)
	{
		Vector3 surfaceLocalPosition = _planet.GetSurfaceLocalPosition(targetLocalPosition + targetLocalPosition.normalized * kOutterBias);
		if(targetLocalPosition.magnitude < surfaceLocalPosition.magnitude)
		{
			targetLocalPosition = surfaceLocalPosition;
		}
		return targetLocalPosition;
	}

	void UpdateOrientation()
	{
		Vector3 newUp = (_host.position - _planet.transform.position).normalized;
		_host.rotation = Quaternion.FromToRotation(_host.up, newUp) * _host.rotation;
	}
	
	void ApplyDrag()
	{
		_currentVelocity *= (1f-_drag);
	}

	PlanetGenerator FindClosestPlanet()
	{
		if(_planet == null)
		{
			_planet = FindObjectOfType<PlanetGenerator>();
		}
		return _planet;
	}

	void OnDrawGizmosSelected()
	{
		if(_planet == null)
		{
			return;
		}

		Vector3 pos = transform.position;

		Vector3 normal = (transform.position - _planet.transform.position).normalized;
		Vector3 target = pos + normal * 6f;

		Gizmos.color = Color.green;
		Gizmos.DrawLine(pos, target);
	}
}
