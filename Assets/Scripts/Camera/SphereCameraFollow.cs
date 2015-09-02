using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class SphereCameraFollow : MonoBehaviour 
{
	[SerializeField]
	Transform _target;

	Quaternion _localToTargetRotation;
	Vector3 _localToTargetDeltaPos;

	void Awake()
	{
		Quaternion targetRotationInverse = Quaternion.Inverse(_target.rotation);
		_localToTargetRotation = targetRotationInverse * transform.rotation;

		Vector3 deltaPos = transform.position - _target.position;
		_localToTargetDeltaPos = targetRotationInverse * deltaPos;
	}

	void Update()
	{
		transform.rotation = _target.rotation * _localToTargetRotation;
		transform.position = _target.position + _target.rotation * _localToTargetDeltaPos;
	}
}
