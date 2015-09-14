using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MQMTech.Mathematics;

[System.Serializable]
public class PlanetGenerator : MonoBehaviorSingleton<PlanetGenerator> 
{
	public enum CoordinateSpace
	{
		Local,
		World
	}

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
	TreeGenerator _treeGenerator;

	[SerializeField]
	float _boxThickness = 1f;

	[SerializeField]
	int _minHeight = 15;

	[SerializeField]
	int _maxHeight = 30;

	[SerializeField]
	float _gravityMagnitude = 9.8f;

	Transform _boxes;

	CustomBoxGenerator _customBoxGenerator = new CustomBoxGenerator();

	Vector3 _corePosition;

	INoise3DGenerator _noiseGenerator;
	HeightFilter _heightFilter;

	int _boxCount = 0;

	float _prevRadiansYX;
	float _prevRadiansYZ;
	float _prevRadiansXZ;
	bool _isFirstTime = true;

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
		_materialGenerator.Init(_maxHeight);
	}

	public void GenerateTerrain(Vector3 worldPosition)
	{
		_corePosition = worldPosition;
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

	// Methods to be refactored into its own Planet
	public BaseBox GetVoxelInPosition(Vector3 worldPosition)
	{
		Vector3 dirToCore = (transform.position - worldPosition).normalized;
		Ray ray = new Ray(worldPosition, dirToCore);

		BaseBox voxel = null;
		RaycastHit hitInfo;
		if(Physics.Raycast(ray, out hitInfo, 999999f, 1<<Layer.Terrain))
		{
			voxel = hitInfo.collider.GetComponentInParent<BaseBox>();
		}
		else
		{
			Debug.Log("Voxel Not Found :(, position: " + worldPosition + ", Direction: " + dirToCore );
		}

		return voxel;
	}

	public Vector3 GetGravity(Vector3 localPosition)
	{
		return (transform.position - localPosition).normalized * _gravityMagnitude;
	}

	public Vector3 ConvertToLocalPosition(Vector3 worldPos)
	{
		Vector3 localPos = worldPos - transform.position;

		Quaternion invRotation = Quaternion.Inverse(transform.rotation);
		localPos = invRotation * localPos;

		return localPos;
	}

	public Vector3 ConvertToWorldPosition(Vector3 localPosition)
	{
		Vector3 worldPos = (transform.rotation * localPosition) + transform.position;
		return worldPos;
	}

	public void GetOrientation(Transform host, out Quaternion rotation)
	{
		Vector3 newUp = (host.position - transform.position).normalized;
		rotation = Quaternion.FromToRotation(host.up, newUp) * host.rotation;
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
		float radius = GetOriginalSurfaceRadius((boxOrientation * Vector3.up).normalized);

		return DoGenerateBox(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius);
	}

	public float GetOriginalSurfaceRadius(Vector3 localNormal)
	{
		return Mathf.Round(_heightFilter.GetHeight(localNormal));
	}

	public Vector3 GetOriginalSurfacePosition(Vector3 localNormal)
	{
		return GetOriginalSurfaceRadius(localNormal)*localNormal;
	}

	public Vector3 GetNormal(Vector3 localPosition)
	{
		return localPosition.normalized;
	}

	public Vector3 GetSurfaceLocalPosition(Vector3 localPosition)
	{
		BaseBox surface = GetVoxelInPosition(ConvertToWorldPosition(localPosition));
		if(surface != null)
		{
			Vector3 surfaceLocalPosition = ConvertToLocalPosition(surface.transform.position);
			float surfaceRadius = surfaceLocalPosition.magnitude;
			
			return localPosition.normalized * surfaceRadius;
		}
		else
		{
			return localPosition;
		}
	}

	public Vector3 GetSurfaceWorldPosition(Vector3 worldPosition)
	{
		BaseBox surface = GetVoxelInPosition(worldPosition);

		Vector3 surfaceLocalPosition = ConvertToLocalPosition(surface.transform.position);
		float surfaceRadius = surfaceLocalPosition.magnitude;

		Vector3 localPosition = ConvertToLocalPosition(worldPosition);
		localPosition = localPosition.normalized * surfaceRadius;

		return ConvertToWorldPosition(localPosition);
	}

	GameObject DoGenerateBox(Quaternion rotationLeft, Quaternion rotationRight, Quaternion boxOrientation, Quaternion rotationCircleStep, float radius)
	{
		Vector3 startPositionBackLeftBottomWS = transform.position + Vector3.up * (radius*0.8f);
		Vector3 startPositionBackLeftTopWS = transform.position + Vector3.up * (radius + _boxThickness);
		
		Vector3 backLeftBottomWS = rotationLeft * startPositionBackLeftBottomWS;
		Vector3 forwardLeftBottomWS = rotationCircleStep * backLeftBottomWS;
		
		Vector3 backLeftTopWS = rotationLeft * startPositionBackLeftTopWS;
		Vector3 forwardLeftTopWS = rotationCircleStep * backLeftTopWS;
		
		Vector3 backRightBottomWS = rotationRight * startPositionBackLeftBottomWS;
		Vector3 forwardRightBottomWS = rotationCircleStep * backRightBottomWS;
		
		Vector3 backRightTopWS = rotationRight * startPositionBackLeftTopWS;
		Vector3 forwardRightTopWS = rotationCircleStep * backRightTopWS;

		Vector3 boxMiddleTopWS = (backLeftTopWS + forwardLeftTopWS + backRightTopWS + forwardRightTopWS) * 0.25f;
		Vector3 boxMiddleBottomWS = (backLeftBottomWS + forwardLeftBottomWS + backRightBottomWS + forwardRightBottomWS) * 0.25f;

		Quaternion inverseBoxOrientation = Quaternion.Inverse(boxOrientation);
		Vector3 backLeftBottom = inverseBoxOrientation*(backLeftBottomWS-boxMiddleTopWS);
		Vector3 forwardLeftBottom = inverseBoxOrientation*(forwardLeftBottomWS-boxMiddleTopWS);
		Vector3 backLeftTop = inverseBoxOrientation*(backLeftTopWS-boxMiddleTopWS);
		Vector3 forwardLeftTop = inverseBoxOrientation*(forwardLeftTopWS-boxMiddleTopWS);
		Vector3 backRightBottom = inverseBoxOrientation*(backRightBottomWS-boxMiddleTopWS);
		Vector3 forwardRightBottom = inverseBoxOrientation*(forwardRightBottomWS-boxMiddleTopWS);
		Vector3 backRightTop = inverseBoxOrientation*(backRightTopWS-boxMiddleTopWS);
		Vector3 forwardRightTop = inverseBoxOrientation*(forwardRightTopWS-boxMiddleTopWS);

		Color lateralColor = mMath.color(1f, 0f, 0f, 0f);
		Color verticalColor = mMath.color(0f, 1f, 0f, 0f);
		Mesh mesh = _customBoxGenerator.Generate(new Vector3[] { backLeftBottom, forwardLeftBottom, backRightBottom, forwardRightBottom, backLeftTop, forwardLeftTop, backRightTop, forwardRightTop }, Quaternion.identity, new Color[]{verticalColor, verticalColor, lateralColor, lateralColor, lateralColor, lateralColor} );
		//_treeGenerator.Genererate(this, boxMiddleTopWS, boxOrientation, GetAverageHalfDistance(backRightTopWS, backLeftTopWS,  forwardRightTopWS, forwardLeftTopWS), boxMiddleTopWS - boxMiddleBottomWS, GetAverageHalfDistance(forwardLeftTopWS, backLeftTopWS,  forwardRightTopWS, backRightTopWS));

		GameObject go = new GameObject();

		_boxCount++;
		go.name = "TerrainBox"+_boxCount.ToString();
		
		MeshFilter mf = go.AddComponent<MeshFilter>();
		mf.mesh = mesh;
		
		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.material = _materialGenerator.Get(radius, boxMiddleTopWS);
		
		BoxCollider collider = go.AddComponent<BoxCollider>();

		go.layer = Layer.Terrain;
		go.transform.position = boxMiddleTopWS;
		go.transform.rotation = boxOrientation;
		
		BaseBox box = go.AddComponent<BaseBox>();
		box.SetGenerationProperties( new BoxGenerationProperties(rotationLeft, rotationRight, boxOrientation, rotationCircleStep, radius) );
		box.Init(this);

		box.transform.SetParent(_boxes, false);
		
		return go;
	}

	Vector3 GetAverageHalfDistance(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1)
	{
		return ((b0+b1)*0.5f - (a0+a1)*0.5f);
	}
}
