using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMesh : MonoBehaviour
{
    public float Radius = 5;	//半径
	public int Segments = 50;	//分割数
 
	private MeshFilter meshFilter;
 
	void Awake()
	{
 
		meshFilter = GetComponent<MeshFilter>();
		meshFilter.mesh = CreateMesh(Radius, Segments);
	}
 
	Mesh CreateMesh(float radius, int segments)
	{
		Mesh mesh = new Mesh ();

        int vlen = 1 + segments;
		Vector3[] vertices = new Vector3[vlen];
		vertices[0] = Vector3.zero;
 
		float angleDegree = 360;
		float angle = Mathf.Deg2Rad * angleDegree;
		float currAngle = angle / 2;
		float deltaAngle = angle / segments;
		for (int i = 1; i < vlen; i++)
		{
			float cosA = Mathf.Cos(currAngle);
			float sinA = Mathf.Sin(currAngle);
			vertices[i] = new Vector3 (cosA * radius, 0, sinA * radius);
			currAngle -= deltaAngle;
		}

        int tlen = segments * 3;
		int[] triangles = new int[tlen];
		for (int i = 0, vi = 1; i < tlen - 3; i += 3, vi++)
		{
			triangles[i] = 0;
			triangles[i + 1] = vi;
			triangles[i + 2] = vi + 1;
		}
		triangles [tlen - 3] = 0;
		triangles [tlen - 2] = vlen - 1;
		triangles [tlen - 1] = 1;

        Vector2[] uvs = new Vector2[vlen];
		for (int i = 0; i < vlen; i++)
		{
			uvs [i] = new Vector2 (vertices[i].x / radius / 2 + 0.5f, vertices[i].z / radius / 2 + 0.5f);
		}

        mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;

		return mesh;
	}
}
