using UnityEngine;
using System.Collections;

[System.Serializable]
public class BoxGenerator : IMeshGenerator
{
	[System.Serializable]
	public struct BoxProperties
	{
		public Vector3 HalfSize;
		public Vector3 Front;
		public Vector3 Up;
		public Vector3 Right;

		public BoxProperties(Vector2 halfSize, Vector3 front, Vector3 up)
		{
			HalfSize = halfSize;
			Front = front.normalized;
			Up = up.normalized;
			Right = Vector3.Cross(Front, Up);
		}
	}

	[SerializeField]
	BoxProperties _properties;

	public void SetProperties(BoxProperties properties)
	{
		_properties = properties;
	}

	public void Generate(Mesh mesh)
	{
		int nvertices = 4 * 6;
		
		Vector3[] vertices = new Vector3[nvertices];
		int vertexIdx = 0;

		Vector2[] uv = new Vector2[nvertices];
		int uvsIdx = 0;

		Vector3[] normals = new Vector3[nvertices];
		int normalIdx = 0;

		int [] indices = new int[6 * 8];
		int indexIdx = 0;

		Mesh tempMesh = new Mesh();
		QuadGenerator quadGenerator = new QuadGenerator();
		QuadGenerator.QuadProperties quadProperties;

		Vector2 HalfSizeXZ = new Vector2(_properties.HalfSize.x, _properties.HalfSize.z);
		Vector2 HalfSizeXY = new Vector2(_properties.HalfSize.x, _properties.HalfSize.y);
		Vector2 HalfSizeZY = new Vector2(_properties.HalfSize.z, _properties.HalfSize.y);

		// Bottom
		Vector3 position = -_properties.Up * _properties.HalfSize.y;
		Vector3 front = -_properties.Up;
		Vector3 up = Vector3.Cross(_properties.Right, front);
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeXZ, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 0);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Top
		position = _properties.Up * _properties.HalfSize.y;
		front = _properties.Up;
		up = Vector3.Cross(_properties.Right, front);
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeXZ, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 4);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Left
		position = -_properties.Right * _properties.HalfSize.x;
		front = -_properties.Right;
		up = _properties.Up;
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeZY, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 8);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Right
		position = _properties.Right * _properties.HalfSize.x;
		front = _properties.Right;
		up = _properties.Up;
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeZY, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 12);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Front
		position = _properties.Front * _properties.HalfSize.z;
		front = _properties.Front;
		up = _properties.Up;
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeXY, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 16);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Bottom
		position = -_properties.Front * _properties.HalfSize.z;
		front = -_properties.Front;
		up = _properties.Up;
		quadProperties = new QuadGenerator.QuadProperties(position, HalfSizeXY, front, up);
		quadGenerator.SetProperties(quadProperties);
		quadGenerator.Generate(tempMesh);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 20);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.triangles = indices;
		mesh.uv = uv;
	}

	int AddVector3(Vector3[] target, Vector3[] source, int offset)
	{
		for (int i = 0; i < source.Length; ++i) 
		{
			target[i + offset] = source[i];
		}
		return source.Length;
	}

	int AddIndices(int[] target, int[] source, int offset, int indexValueOffset)
	{
		for (int i = 0; i < source.Length; ++i) 
		{
			target[i + offset] = source[i] + indexValueOffset;
		}
		return source.Length;
	}

	int AddUVs(Vector2[] target, Vector2[] source, int offset)
	{
		for (int i = 0; i < source.Length; ++i) 
		{
			target[i + offset] = source[i];
		}
		return source.Length;
	}
}
