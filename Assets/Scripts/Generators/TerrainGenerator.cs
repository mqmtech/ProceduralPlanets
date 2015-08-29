using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Singleton<T> where T : class, new()
{
	static T _instance = default(T);
	public static T Instance
	{
		get
		{
			if(_instance == default(T))
			{
				_instance = new T();
			}
			return _instance;
		}
	}
}

public abstract class MonoBehaviorSingleton<T> : MonoBehaviour where T : MonoBehaviour, new()
{
	static T _instance = default(T);
	public static T Instance
	{
		get
		{
			if(_instance == default(T))
			{
				_instance = FindObjectOfType<T>();
				if(_instance == default(T))
				{
					GameObject go = new GameObject();
					go.name = typeof(T).ToString();
					_instance = go.AddComponent<T>();
				}
			}

			return _instance;
		}
	}
}

[System.Serializable]
public class TerrainGenerator : MonoBehaviorSingleton<TerrainGenerator> 
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
	Material _material;

	[SerializeField]
	QuadGenerator _quadGenerator;

	[SerializeField]
	BoxGenerator _boxGenerator;

	[SerializeField]
	float _boxThickness = 1f;

	[SerializeField]
	int _minHeight = 5;

	[SerializeField]
	int _maxHeight = 10;

	Transform _boxes;

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
		_boxes = transform.GetChild(0);
	}

	public void GenerateTerrain(Vector3 corePosition)
	{
		_corePosition = corePosition;

		int maxLayerIdx = Mathf.RoundToInt(_maxHeight);
		GenerateLayer((float) _maxHeight, 8f);
	}

	public void OnBoxDestroyed(BaseBox box)
	{
		TerrainGenerator.BoxGenerationProperties properties = box.GenerationProperties;
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
		float radius = Mathf.Round(_heightFilter.GetHeight((boxOrientation * Vector3.up).normalized));

		return DoGenerateBox(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius);
	}

	GameObject DoGenerateBox(Quaternion rotationLeft, Quaternion rotationRight, Quaternion boxOrientation, Quaternion rotationCircleStep, float radius)
	{
		Vector3 startPositionBackLeftBottom = Vector3.up;
		Vector3 startPositionBackLeftTop = Vector3.up * (radius + _boxThickness);
		
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
		
		Collider collider = go.AddComponent<MeshCollider>();
		
		BaseBox box = go.AddComponent<BaseBox>();
		box.SetGenerationProperties( new BoxGenerationProperties(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius) );

		box.transform.SetParent(_boxes, false);
		
		return go;
	}
}
