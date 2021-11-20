using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    Vector3[] normals;
    int[] triangles;

    public int xSize = 20;
    public int zSize = 20;
    public float ratio = 1.5f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    // Start is called before the first frame update
    void Start()
    {
        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        CreateShape();
        UpdateMesh();
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    void CreateShape()
    {
        int vertexNum = 6 * (xSize + 1) * (zSize + 1);
        vertices = new Vector3[vertexNum];
        triangles = new int[vertexNum];
        normals = new Vector3[vertexNum];

        for (int i = 0; i < vertexNum; i++)
        {
            vertices[i] = Vector3.zero;
            triangles[i] = 0;
            normals[i] = Vector3.zero;
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                int idx0 = 6 * (x + z * xSize);
                int idx1 = idx0 + 1;
                int idx2 = idx0 + 2;
                int idx3 = idx0 + 3;
                int idx4 = idx0 + 4;
                int idx5 = idx0 + 5;

                var vert00 = GetRandVec(x, z, offsetX, offsetY);
                var vert01 = GetRandVec(x, z + 1, offsetX, offsetY);
                var vert10 = GetRandVec(x + 1, z, offsetX, offsetY);
                var vert11 = GetRandVec(x + 1, z + 1, offsetX, offsetY);

                vertices[idx0] = vert00;
                vertices[idx1] = vert01;
                vertices[idx2] = vert11;
                vertices[idx3] = vert00;
                vertices[idx4] = vert11;
                vertices[idx5] = vert10;

                Vector3 normal000111 = Vector3.Cross(vert01 - vert00, vert11 - vert00).normalized;
                Vector3 normal001011 = Vector3.Cross(vert11 - vert00, vert10 - vert00).normalized;

                normals[idx0] = normal000111;
                normals[idx1] = normal000111;
                normals[idx2] = normal000111;
                normals[idx3] = normal001011;
                normals[idx4] = normal001011;
                normals[idx5] = normal001011;

                triangles[idx0] = idx0;
                triangles[idx1] = idx1;
                triangles[idx2] = idx2;
                triangles[idx3] = idx3;
                triangles[idx4] = idx4;
                triangles[idx5] = idx5;
            }
        }
    }

    private Vector3 GetRandVec(int x, int z, float offsetX, float offsetY)
    {
        return new Vector3(x, Mathf.PerlinNoise(x * offsetX, z * offsetY) * ratio, z);
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
}