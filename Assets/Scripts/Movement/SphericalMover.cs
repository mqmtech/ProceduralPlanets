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
	}

	public bool Move(Vector3 dirLocalToFloor)
	{
//		BaseBox voxel =_planet.GetVoxelInPosition(_host.position);
//		if(voxel == null)
//		{
//			return false;
//		}

		//Vector3 stepMagnitude = (dirLocalToFloor.x * voxel.transform.right + dirLocalToFloor.z * voxel.transform.forward + dirLocalToFloor.y * voxel.transform.up) * _speed * Time.deltaTime;
		Vector3 right;
		Vector3 up;
		Vector3 forward;
		_planet.GetAxisFromPosition(_host.position, PlanetGenerator.CoordinateSpace.World, out right, out up, out forward);

		Debug.Log("Right: " + right + ", Forward: " + forward);


		Vector3 stepMagnitude = (dirLocalToFloor.x * right + dirLocalToFloor.z * forward + dirLocalToFloor.y * up) * _speed * Time.deltaTime;
		Vector3 nextPosition = _host.position + stepMagnitude;
		_host.position = nextPosition;

		return true;
	}
}
