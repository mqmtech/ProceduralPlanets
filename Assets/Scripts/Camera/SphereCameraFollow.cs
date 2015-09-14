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
		Vector3 targetPosition;
		Quaternion targetRotation;
		if(transform.position.magnitude > 75f)
		{
			targetRotation = _localToTargetRotation;
			targetPosition =  _target.position + _localToTargetDeltaPos;;
		}
		else
		{
			targetRotation = _target.rotation * _localToTargetRotation;
			targetPosition =  _target.position + _target.rotation * _localToTargetDeltaPos;;
		}

		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);
	}
}
