using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CustomQuadGenerator
{
	// Bottom Left, Top Left, Top Right, Bottom Right
	public Mesh Generate(List<Vector3> positions, Vector3 normal)
	{
		Mesh mesh = new Mesh();
		Generate(mesh, positions, normal);

		return mesh;
	}

	public void Generate(Mesh mesh, List<Vector3> positions, Vector3 normal)
	{
		int nvertices = 4;
		
		Vector3[] vertices = new Vector3[nvertices];
		Vector3[] normals = new Vector3[nvertices];
		Vector2 [] uvs = new Vector2[nvertices];
		int [] indices = {0, 1, 2, 0, 2, 3};
		
		vertices[0] = positions[0];
		vertices[1] = positions[1];
		vertices[2] = positions[2];
		vertices[3] = positions[3];

		//Vector3 normal = Vector3.Cross((positions[1]-positions[0]).normalized, (positions[3]-positions[0]).normalized);

		normals[0] = normal;
		normals[1] = normal;
		normals[2] = normal;
		normals[3] = normal;
		
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
