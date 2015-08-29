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

	[SerializeField]
	int _minHeight = 5;

	[SerializeField]
	int _maxHeight = 10;

	CustomBoxGenerator _customBoxGenerator = new CustomBoxGenerator();

	[SerializeField]
	Properties _properties;

	Vector3 _corePosition;

	INoise3DGenerator _noiseGenerator;
	HeightFilter _heightFilter;

	void Start()
	{
		Init();
		GenerateTerrain(transform.position);
	}

	public void Init()
	{
		_noiseGenerator = new SinusNoiseGenerator();
		_heightFilter = new HeightFilter();
		_heightFilter.Init(_minHeight, _maxHeight, _noiseGenerator);
	}

	public void GenerateTerrain(Vector3 corePosition)
	{
		_corePosition = corePosition;

		int maxLayerIdx = Mathf.RoundToInt(_maxHeight);
//		for (int layerIdx = 1; layerIdx <= maxLayerIdx; ++layerIdx)
//		{
//			GenerateLayer((float) layerIdx, 8f, 1.0f, layerIdx);
//		}
		GenerateLayer((float) _maxHeight, 8f, 1.0f, 0);
	}

	void GenerateLayer(float radius, float xzDivisionFactor, float thickness, int currentLayerIdx)
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
				GenerateBox(currentBoxesAngle, currentBoxesAngle + boxesAngleStep, radius, thickness, cicleForward, circleRotationStep, circleRotationHalf, currentLayerIdx);
				currentBoxesAngle += boxesAngleStep;
			}
		}
	}

	GameObject GenerateBox(float angleStart, float angleEnd, float radius, float boxThickness, Vector3 circleForward, Quaternion rotationCircleStep, Quaternion totalCircleRotationHalf, int currentLayerIdx)
	{
		Quaternion rotationLeft = Quaternion.AngleAxis(angleStart, circleForward);
		Quaternion rotationRight = Quaternion.AngleAxis(angleEnd, circleForward);
		Quaternion boxOrientation = Quaternion.AngleAxis((angleStart+angleEnd)*0.5f, circleForward) * totalCircleRotationHalf;
		radius = _heightFilter.GetHeight((boxOrientation * Vector3.up).normalized);

		//Vector3 startPositionBackLeftBottom = Vector3.up * radius;
		Vector3 startPositionBackLeftBottom = Vector3.up;
		Vector3 startPositionBackLeftTop = Vector3.up * (radius + boxThickness);

		Vector3 backLeftBottom = rotationLeft * startPositionBackLeftBottom;
		Vector3 forwardLeftBottom = rotationCircleStep * backLeftBottom;

		Vector3 backLeftTop = rotationLeft * startPositionBackLeftTop;
		Vector3 forwardLeftTop = rotationCircleStep * backLeftTop;

		Vector3 backRightBottom = rotationRight * startPositionBackLeftBottom;
		Vector3 forwardRightBottom = rotationCircleStep * backRightBottom;

		Vector3 backRightTop = rotationRight * startPositionBackLeftTop;
		Vector3 frontRightTop = rotationCircleStep * backRightTop;

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
