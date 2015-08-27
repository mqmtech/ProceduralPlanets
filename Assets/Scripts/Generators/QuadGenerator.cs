using UnityEngine;
using System.Collections;

[System.Serializable]
public class QuadGenerator : IMeshGenerator
{
	[System.Serializable]
	public struct QuadProperties
	{
		public Vector3 Position;
		public Vector2 HalfSize;
		public Vector3 Front;
		public Vector3 Up;

		public Vector3 Right;

		public QuadProperties(Vector3 position, Vector2 halfSize, Vector3 front, Vector3 up)
		{
			Position = position;
			HalfSize = halfSize;
			Front = front.normalized;
			Up = up.normalized;
			Right = Vector3.Cross(Front, Up);
		}
	}

	[SerializeField]
	QuadProperties _properties;

	public void SetProperties(QuadProperties properties)
	{
		_properties = properties;
	}

	public void Generate(Mesh mesh)
	{
		int nvertices = 4;

		Vector3[] vertices = new Vector3[nvertices];
		Vector3[] normals = new Vector3[nvertices];
		Vector2 [] uvs = new Vector2[nvertices];
		int [] indices = {0, 1, 2, 0, 2, 3};

		vertices[0] = _properties.Position + (-_properties.Right*_properties.HalfSize.x - _properties.Up*_properties.HalfSize.y);
		vertices[1] = _properties.Position + (-_properties.Right*_properties.HalfSize.x + _properties.Up*_properties.HalfSize.y);
		vertices[2] = _properties.Position + (_properties.Right*_properties.HalfSize.x + _properties.Up*_properties.HalfSize.y);
		vertices[3] = _properties.Position + (_properties.Right*_properties.HalfSize.x -_properties.Up*_properties.HalfSize.y);

		normals[0] = _properties.Front;
		normals[1] = _properties.Front;
		normals[2] = _properties.Front;
		normals[3] = _properties.Front;

		uvs[0] = new Vector2(0, 0);
		uvs[1] = new Vector2(0, 1);
		uvs[2] = new Vector2(1, 1);
		uvs[3] = new Vector2(1, 0);

		mesh.vertices = vertices;
		mesh.triangles = indices;
		mesh.normals = normals;
		mesh.uv = uvs;
	}
}
