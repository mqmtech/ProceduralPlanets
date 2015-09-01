using UnityEngine;
using System.Collections;
using MQMTech.Mathematics;

using vec4 = UnityEngine.Vector4;
using vec3 = UnityEngine.Vector3;
using vec2 = UnityEngine.Vector2;

[System.Serializable]
public class TreeGenerator 
{
	static int _treeCount;

	[SerializeField]
	float _lengthXZFactor = 1.0f;

	[SerializeField]
	float _lengthYFactor = 1.0f;

	[SerializeField]
	float _moiseScale = 10.0f;

	[SerializeField]
	float _treeProbability = 0.25f;

	[SerializeField]
	Material _material;

	PlanetGenerator _generator;

	Vector3 _forwardWS;
	Vector3 _upWS;
	Vector3 _rightWS;

	Vector3 _forward;
	Vector3 _up;
	Vector3 _right;

	Quaternion _boxOrientation;
	Quaternion _inverseBoxOrientation;

	Vector3 _floorPosition;

	CustomBoxGenerator _customBoxGenerator = new CustomBoxGenerator();

	PerlinNoiseGenerator _perlinNoiseGenerator = new PerlinNoiseGenerator();

	public GameObject Genererate(PlanetGenerator generator, Vector3 floorPosition, Quaternion boxOrientation, Vector3 forwardWS, Vector3 upWS, Vector3 rightWS)
	{
		float randomNumber = _perlinNoiseGenerator.Generate(floorPosition * _moiseScale);
		if(randomNumber > _treeProbability)
		{
			return null;
		}

		_generator = generator;

		_floorPosition = floorPosition;

		_boxOrientation = boxOrientation;
		_inverseBoxOrientation = Quaternion.Inverse(_boxOrientation);

		_forwardWS = forwardWS * _lengthXZFactor;
		_upWS = upWS * _lengthYFactor;
		_rightWS = rightWS * _lengthXZFactor;

		_forward = _inverseBoxOrientation * _forwardWS;
		_up = _inverseBoxOrientation * _upWS;
		_right = _inverseBoxOrientation * _rightWS;

		Vector3 floorPivotPos = _floorPosition -rightWS*0.5f -_forwardWS*0.5f;
		// Trunk
		DoGenererateBox(floorPivotPos);
		DoGenererateBox(floorPivotPos+_upWS);

		DoGenererateBox(floorPivotPos+_upWS -_forwardWS*2f -_rightWS);
		DoGenererateBox(floorPivotPos+_upWS -_forwardWS*2f);
		DoGenererateBox(floorPivotPos+_upWS -_forwardWS*2f +_rightWS);

		DoGenererateBox(floorPivotPos+_upWS +_forwardWS*2f -_rightWS);
		DoGenererateBox(floorPivotPos+_upWS +_forwardWS*2f);
		DoGenererateBox(floorPivotPos+_upWS +_forwardWS*2f +_rightWS);


		DoGenererateBox(floorPivotPos+_upWS -_forwardWS -_rightWS*2f);
		DoGenererateBox(floorPivotPos+_upWS -_rightWS*2f);
		DoGenererateBox(floorPivotPos+_upWS +_forwardWS -_rightWS*2f);

		DoGenererateBox(floorPivotPos+_upWS -_forwardWS +_rightWS*2f);
		DoGenererateBox(floorPivotPos+_upWS +_rightWS*2f);
		DoGenererateBox(floorPivotPos+_upWS +_forwardWS +_rightWS*2f);

		DoGenererateBox(floorPivotPos+_upWS*2f);
		DoGenererateBox(floorPivotPos+_upWS*2f -_forwardWS);
		DoGenererateBox(floorPivotPos+_upWS*2f +_forwardWS);
		DoGenererateBox(floorPivotPos+_upWS*2f -_rightWS);
		DoGenererateBox(floorPivotPos+_upWS*2f +_rightWS);

		return null;
	}

	GameObject DoGenererateBox(Vector3 worldPosition)
	{
		Vector3 backLeftBottom = Vector3.zero;
		Vector3 forwardLeftBottom = _forward;
		Vector3 backRightBottom = _right;
		Vector3 forwardRightBottom = _forward +_right;
		
		Vector3 backLeftTop = _up;
		Vector3 forwardLeftTop = _forward +_up;
		Vector3 backRightTop = _right +_up;
		Vector3 forwardRightTop = _forward +_right +_up;
		
		Color lateralColor = mMath.color(1f, 0f, 0f, 0f);
		Color verticalColor = mMath.color(0f, 1f, 0f, 0f);
		
		// Create trunk
		Mesh mesh = _customBoxGenerator.Generate(new Vector3[] { backLeftBottom, forwardLeftBottom, backRightBottom, forwardRightBottom, backLeftTop, forwardLeftTop, backRightTop, forwardRightTop }, Quaternion.identity, new Color[]{verticalColor, verticalColor, lateralColor, lateralColor, lateralColor, lateralColor} );
		
		GameObject go = new GameObject();
		
		_treeCount++;
		go.name = "TerrainBox" + _treeCount.ToString();
		
		MeshFilter mf = go.AddComponent<MeshFilter>();
		mf.mesh = mesh;
		
		MeshRenderer mr = go.AddComponent<MeshRenderer>();
		mr.material = _material;
		
		BoxCollider collider = go.AddComponent<BoxCollider>();

		go.layer = Layer.Terrain;
		go.transform.position = worldPosition;
		go.transform.rotation = _boxOrientation;
		
		return go;
	}
}
