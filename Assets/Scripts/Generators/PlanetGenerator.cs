using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MQMTech.Mathematics;

[System.Serializable]
public class PlanetGenerator : MonoBehaviorSingleton<PlanetGenerator> 
{
	[System.Serializable]
	public struct Properties
	{
		public float BlockSize;
	}

	[System.Serializable]
	public struct BoxGenerationProperties
	{
		public Quaternion LeftRotation;
		public Quaternion RightRotation;
		public Quaternion BoxOrientation;
		public Quaternion RotationCircleStep;
		public float Radius;

		public BoxGenerationProperties(Quaternion iLeftRotation, Quaternion iRightRotation, Quaternion iBoxOrientation, Quaternion iRotationCircleStep, float iRadius)
		{
			LeftRotation = iLeftRotation;
			RightRotation = iRightRotation;
			BoxOrientation = iBoxOrientation;
			RotationCircleStep = iRotationCircleStep;
			Radius = iRadius;
		}
	}

	[SerializeField]
	MaterialGenerator _materialGenerator;

	[SerializeField]
	float _boxThickness = 1f;

	[SerializeField]
	int _minHeight = 5;

	[SerializeField]
	int _maxHeight = 10;

	Transform _boxes;

	CustomBoxGenerator _customBoxGenerator = new CustomBoxGenerator();

	Vector3 _corePosition;

	INoise3DGenerator _noiseGenerator;
	HeightFilter _heightFilter;

	int _boxCount = 0;

	void Start()
	{
		Init();
		GenerateTerrain(transform.position);
	}

	public void Init()
	{
		_noiseGenerator = new PerlinNoiseGenerator();
		_heightFilter = new HeightFilter();
		_heightFilter.Init(_minHeight, _maxHeight, _noiseGenerator);
		_boxes = transform.GetChild(0);
	}

	public void GenerateTerrain(Vector3 corePosition)
	{
		_corePosition = corePosition;
		_boxCount = 0;

		int maxLayerIdx = Mathf.RoundToInt(_maxHeight);
		GenerateLayer((float) _maxHeight, 8f);
	}

	public void OnBoxDestroyed(BaseBox box)
	{
		PlanetGenerator.BoxGenerationProperties properties = box.GenerationProperties;
		properties.Radius--;

		if(properties.Radius > 1f)
		{
			DoGenerateBox(properties.LeftRotation, properties.RightRotation, properties.BoxOrientation, properties.RotationCircleStep, properties.Radius);
		}
	}

	void GenerateLayer(float maxRadius, float xzDivisionFactor)
	{
		float hDivisions = Mathf.Round( Mathf.Pow(maxRadius, 0.8f) * xzDivisionFactor);
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
				GenerateBox(currentBoxesAngle, currentBoxesAngle + boxesAngleStep, cicleForward, circleRotationStep, circleRotationHalf);
				currentBoxesAngle += boxesAngleStep;
			}
		}
	}

	GameObject GenerateBox(float angleStart, float angleEnd, Vector3 circleForward, Quaternion rotationCircleStep, Quaternion totalCircleRotationHalf)
	{
		Quaternion rotationLeft = Quaternion.AngleAxis(angleStart, circleForward);
		Quaternion rotationRight = Quaternion.AngleAxis(angleEnd, circleForward);
		Quaternion boxOrientation = Quaternion.AngleAxis((angleStart+angleEnd)*0.5f, circleForward) * totalCircleRotationHalf;
		float radius = GetRadiusFromNormal((boxOrientation * Vector3.up).normalized);

		return DoGenerateBox(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius);
	}

	public float GetRadiusFromNormal(Vector3 normal)
	{
		return Mathf.Round(_heightFilter.GetHeight(normal));
	}

	GameObject DoGenerateBox(Quaternion rotationLeft, Quaternion rotationRight, Quaternion boxOrientation, Quaternion rotationCircleStep, float radius)
	{
		Vector3 startPositionBackLeftBottom = Vector3.up * (radius );
		Vector3 startPositionBackLeftTop = Vector3.up * (radius + _boxThickness);
		
		Vector3 backLeftBottom = rotationLeft * startPositionBackLeftBottom;
		Vector3 forwardLeftBottom = rotationCircleStep * backLeftBottom;
		
		Vector3 backLeftTop = rotationLeft * startPositionBackLeftTop;
		Vector3 forwardLeftTop = rotationCircleStep * backLeftTop;
		
		Vector3 backRightBottom = rotationRight * startPositionBackLeftBottom;
		Vector3 forwardRightBottom = rotationCircleStep * backRightBottom;
		
		Vector3 backRightTop = rotationRight * startPositionBackLeftTop;
		Vector3 frontRightTop = rotationCircleStep * backRightTop;

		//Mesh mesh = _customBoxGenerator.Generate(new List<Vector3>() { backLeftBottom, forwardLeftBottom, backRightBottom, forwardRightBottom, backLeftTop, forwardLeftTop, backRightTop, frontRightTop }, boxOrientation);
		Vector3 boxPivotPosition = (backLeftTop + forwardLeftTop + backRightTop + frontRightTop) * 0.25f;
		Quaternion inverseBoxOrientation = Quaternion.Inverse(boxOrientation);
		backLeftBottom = inverseBoxOrientation*(backLeftBottom-boxPivotPosition);
		forwardLeftBottom = inverseBoxOrientation*(forwardLeftBottom-boxPivotPosition);
		backLeftTop = inverseBoxOrientation*(backLeftTop-boxPivotPosition);
		forwardLeftTop = inverseBoxOrientation*(forwardLeftTop-boxPivotPosition);
		backRightBottom = inverseBoxOrientation*(backRightBottom-boxPivotPosition);
		forwardRightBottom = inverseBoxOrientation*(forwardRightBottom-boxPivotPosition);
		backRightTop = inverseBoxOrientation*(backRightTop-boxPivotPosition);
		frontRightTop = inverseBoxOrientation*(frontRightTop-boxPivotPosition);

		Color lateralColor = mMath.color(1f, 0f, 0f, 0f);
		Color verticalColor = mMath.color(0f, 1f, 0f, 0f);
		Mesh mesh = _customBoxGenerator.Generate(new Vector3[] { backLeftBottom, forwardLeftBottom, backRightBottom, forwardRightBottom, backLeftTop, forwardLeftTop, backRightTop, frontRightTop }, Quaternion.identity, new Color[]{verticalColor, verticalColor, lateralColor, lateralColor, lateralColor, lateralColor} );

		GameObject go = new GameObject();

		_boxCount++;
		go.name = "TerrainBox"+_boxCount.ToString();
		
		MeshFilter mf = go.AddComponent<MeshFilter>();
		mf.mesh = mesh;
		
		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.material = _materialGenerator.Get(radius, boxPivotPosition);
		
		BoxCollider collider = go.AddComponent<BoxCollider>();

		go.transform.position = boxPivotPosition;
		go.transform.rotation = boxOrientation;
		//go.isStatic = true;
		
		BaseBox box = go.AddComponent<BaseBox>();
		box.SetGenerationProperties( new BoxGenerationProperties(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius) );

		box.transform.SetParent(_boxes, false);
		
		return go;
	}
}
