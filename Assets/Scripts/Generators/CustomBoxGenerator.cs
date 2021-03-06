﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MQMTech.Mathematics;

using vec4 = UnityEngine.Vector4;
using vec3 = UnityEngine.Vector3;
using vec2 = UnityEngine.Vector2;

[System.Serializable]
public class CustomBoxGenerator
{
	// 0 Back, Left, Bottom
	// 1 Front, Left, Bottom
	// 2 Back, Right, Bottom
	// 3 Front, Right, Bottom
	
	// 4 Back, Left, Top
	// 5 Front, Left, Top
	// 6 Back, Right, Top
	// 7 Front, Right, Top
	public Mesh Generate(Vector3 [] positions, Quaternion orientation, Color[] colorPerFace)
	{
		int nvertices = 4 * 6;
		
		Vector3[] vertices = new Vector3[nvertices];
		int vertexIdx = 0;

		Color[] colors = new Color[nvertices];
		int colorsIdx = 0;

		Vector2[] uv = new Vector2[nvertices];
		int uvsIdx = 0;

		Vector3[] normals = new Vector3[nvertices];
		int normalIdx = 0;

		int [] indices = new int[6 * 8];
		int indexIdx = 0;

		Mesh tempMesh = new Mesh();
		CustomQuadGenerator quadGenerator = new CustomQuadGenerator();

		Vector3 normal;
		// Bottom
		normal = orientation * Vector3.down;
		quadGenerator.Generate(tempMesh, new Vector3 []{ positions[1], positions[0], positions[2], positions[3] }, new Color []{ colorPerFace[0], colorPerFace[0], colorPerFace[0], colorPerFace[0] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 0);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Top
		normal = orientation * Vector3.up;
		quadGenerator.Generate(tempMesh,new Vector3 []{ positions[4], positions[5], positions[7], positions[6] }, new Color []{ colorPerFace[1], colorPerFace[1], colorPerFace[1], colorPerFace[1] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 4);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Left
		normal = orientation * Vector3.left;
		quadGenerator.Generate(tempMesh,new Vector3 []{ positions[1], positions[5], positions[4], positions[0] }, new Color []{ colorPerFace[2], colorPerFace[2], colorPerFace[2], colorPerFace[2] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 8);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Right
		normal = orientation * Vector3.right;
		quadGenerator.Generate(tempMesh,new Vector3 []{ positions[2], positions[6], positions[7], positions[3] }, new Color []{ colorPerFace[3], colorPerFace[3], colorPerFace[3], colorPerFace[3] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 12);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Back
		normal = orientation * Vector3.back;
		quadGenerator.Generate(tempMesh,new Vector3 []{ positions[0], positions[4], positions[6], positions[2] }, new Color []{ colorPerFace[4], colorPerFace[4], colorPerFace[4], colorPerFace[4] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 16);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		// Front
		normal = orientation * Vector3.forward;
		quadGenerator.Generate(tempMesh,new Vector3 []{ positions[3], positions[7], positions[5], positions[1] }, new Color []{ colorPerFace[5], colorPerFace[5], colorPerFace[5], colorPerFace[5] }, normal);
		vertexIdx += AddVector3(vertices, tempMesh.vertices, vertexIdx);
		colorsIdx += AddColors(colors, tempMesh.colors, colorsIdx);
		normalIdx += AddVector3(normals, tempMesh.normals, normalIdx);
		indexIdx += AddIndices(indices, tempMesh.triangles, indexIdx, 20);
		uvsIdx += AddUVs(uv, tempMesh.uv, uvsIdx);

		Mesh mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.triangles = indices;
		mesh.colors = colors;
		mesh.uv = uv;

		return mesh;
	}

	int AddVector3(Vector3[] target, Vector3[] source, int offset)
	{
		for (int i = 0; i < source.Length; ++i) 
		{
			target[i + offset] = source[i];
		}
		return source.Length;
	}

	int AddColors(Color[] target, Color[] source, int offset)
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
