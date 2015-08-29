using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour 
{
	[SerializeField]
	Vector3 _axis = Vector3.up;

	[SerializeField]
	float _degreeSpeed = 1f;

	Quaternion _rotation;

	void Awake()
	{
		_rotation = Quaternion.AngleAxis(_degreeSpeed, _axis);
	}

	void Update()
	{
		Quaternion finalRotation = _rotation * transform.localRotation;

		transform.localRotation = finalRotation;
	}
}
