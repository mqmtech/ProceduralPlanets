using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CustomQuadGenerator
{
	// Bottom Left, Top Left, Top Right, Bottom Right
	public Mesh Generate(Vector3 [] vertices, Color [] colors, Vector3 normal)
	{
		Mesh mesh = new Mesh();
		Generate(mesh, vertices, colors, normal);

		return mesh;
	}

	public void Generate(Mesh mesh, Vector3 [] vertices, Color [] colors, Vector3 normal)
	{
		int nvertices = 4;
		
		Vector3[] normals = new Vector3[nvertices];
		Vector2 [] uvs = new Vector2[nvertices];
		int [] indices = {0, 1, 2, 0, 2, 3};

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
		mesh.colors = colors;
		mesh.uv = uvs;
	}
}
