using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TerrainGenerator : MonoBehaviour 
{
	[SerializeField]
	public struct Properties
	{
		public float BlockSize;
	}

	[SerializeField]
	Material _material;

	[SerializeField]
	QuadGenerator _quadGenerator;

	[SerializeField]
	BoxGenerator _boxGenerator;

	CustomBoxGenerator _customBoxGenerator = new CustomBoxGenerator();

	[SerializeField]
	Properties _properties;

	Vector3 _corePosition;

	void Start()
	{
		GenerateTerrain(transform.position);
	}

	public void GenerateTerrain(Vector3 corePosition)
	{
		_corePosition = corePosition;

		for (int layerIdx = 1; layerIdx < 10; ++layerIdx)
		{
			GenerateLayer((float) layerIdx, 8f, 1.0f);
		}
	}

	void GenerateLayer(float radius, float xzDivisionFactor, float thickness)
	{
		float hDivisions = Mathf.Round( Mathf.Pow(radius, 0.8f) * xzDivisionFactor);
		float vDisisionsHalf = hDivisions * 0.5f;

		float cicleAngleStep = -360f / hDivisions;
		float currentCircleAngle = 0f;
		Quaternion circleRotationStep = Quaternion.AngleAxis(cicleAngleStep, Vector3.up);
		for (int circleIdx = 0; circleIdx < (int)hDivisions; ++circleIdx, currentCircleAngle += cicleAngleStep) 
		{
			Quaternion circleRotation = Quaternion.AngleAxis(currentCircleAngle, Vector3.up);
			Quaternion circleRotationHalf = Quaternion.AngleAxis((currentCircleAngle+cicleAngleStep)*0.5f, Vector3.up);
			Vector3 cicleForward = circleRotation * Vector3.forward;

			float boxesAngleStep = -180f / vDisisionsHalf;
			float currentBoxesAngle = 0f;
			for (int boxIdx = 0; boxIdx < (int) vDisisionsHalf; ++boxIdx) 
			{
				GenerateBox(currentBoxesAngle, currentBoxesAngle + boxesAngleStep, radius, thickness, cicleForward, circleRotationStep, circleRotationHalf);
				currentBoxesAngle += boxesAngleStep;
			}
		}
	}



	GameObject GenerateBox(float angleStart, float angleEnd, float radius, float boxThickness, Vector3 circleForward, Quaternion rotationCircleStep, Quaternion totalCircleRotationHalf)
	{
		Vector3 startPositionBackLeftBottom = Vector3.up * radius;
		Vector3 startPositionBackLeftTop = Vector3.up * (radius + boxThickness);
		
		Quaternion rotationLeft = Quaternion.AngleAxis(angleStart, circleForward);

		Vector3 backLeftBottom = rotationLeft * startPositionBackLeftBottom;
		Vector3 forwardLeftBottom = rotationCircleStep * backLeftBottom;

		Vector3 backLeftTop = rotationLeft * startPositionBackLeftTop;
		Vector3 forwardLeftTop = rotationCircleStep * backLeftTop;
		
		Quaternion rotationRight = Quaternion.AngleAxis(angleEnd, circleForward);

		Vector3 backRightBottom = rotationRight * startPositionBackLeftBottom;
		Vector3 forwardRightBottom = rotationCircleStep * backRightBottom;

		Vector3 backRightTop = rotationRight * startPositionBackLeftTop;
		Vector3 frontRightTop = rotationCircleStep * backRightTop;

		Quaternion boxOrientation = Quaternion.AngleAxis((angleStart+angleEnd)*0.5f, circleForward) * totalCircleRotationHalf;
		Mesh mesh = _customBoxGenerator.Generate(new List<Vector3>() { backLeftBottom, forwardLeftBottom, backRightBottom, forwardRightBottom, backLeftTop, forwardLeftTop, backRightTop, frontRightTop }, boxOrientation);
		GameObject go = new GameObject();

		MeshFilter mf = go.AddComponent<MeshFilter>();
		mf.mesh = mesh;

		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.material = _material;

		BoxCollider collider = go.AddComponent<BoxCollider>();
		collider.isTrigger = true;

		BaseBox box = go.AddComponent<BaseBox>();

		return go;
	}

	GameObject Spawn(Vector3 position)
	{
		GameObject gameObject = new GameObject();

		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		mr.material = _material;

		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.mesh = GenerateMesh();
		
		return gameObject;
	}

	Mesh GenerateMesh()
	{
		Mesh mesh = new Mesh();
		//_quadGenerator.Generate(mesh);
		_boxGenerator.Generate(mesh);
		return mesh;
	}
}
