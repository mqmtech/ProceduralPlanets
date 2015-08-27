using UnityEngine;
using System.Collections;

public class TestGenerator : MonoBehaviour 
{
	[SerializeField]
	Material _material;

	[SerializeField]
	QuadGenerator _quadGenerator;

	[SerializeField]
	BoxGenerator _boxGenerator;

	void Start()
	{
		Spawn(transform.position);
	}

	GameObject Spawn(Vector3 position)
	{
		GameObject gameObject = new GameObject();

		MeshRenderer mr = gameObject.AddComponent<MeshRenderer>();
		mr.material = _material;

		MeshFilter mf = gameObject.AddComponent<MeshFilter>();
		mf.mesh = GenerateMesh();

		BaseBox box = gameObject.AddComponent<BaseBox>();
		
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
