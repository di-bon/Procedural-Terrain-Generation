using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class TerrainGenerator : MonoBehaviour
{
    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;

    private float[,] noiseMap;

    private const int MAX_X = 100;
    private const int MAX_Z = 100;

    public int seed = 0;

    public Vector3 offset = new Vector3(0, 0, 0);

    [Range(1, MAX_X)]
    public int xSize = 20;

    [Range(1, MAX_Z)]
    public int zSize = 20;

    [Range(.0001f, 50f)]
    public float scale = 1f;

    [Range(0f, 1f)]
    public float frequency = .0069f;

    [Range(.0001f, 10f)]
    public float amplitude = 3f;

    [Range(1, 10)]
    public int octaves = 4;

    [Range(1f, 10f)]
    public float lacunarity = 3.81f;

    [Range(.0001f, 1f)]
    public float persistance = .593f;

    [Range(.0001f, 5f)]
    public float baseMultiplier = 1.84f;

    [Range(.0001f, 10f)]
    public float exponent = 2.75f;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
        DisplayNoiseMap(noiseMap);
    }


    private void OnValidate()
    {
        CreateShape();
        UpdateMesh();
        DisplayNoiseMap(noiseMap);
    }

    private void CreateShape()
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        noiseMap = Noise.GenerateNoiseMap(seed, offset, xSize, zSize, scale, frequency, amplitude, octaves, persistance, lacunarity, baseMultiplier, exponent);

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = noiseMap[x, z];
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * zSize * 6];     // 6 vertices (i.e. 2 triangles) for each square

        for (int tris = 0, vert = 0, z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + xSize + 1;
                triangles[tris + 4] = vert + xSize + 2;
                triangles[tris + 5] = vert + 1;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    private void UpdateMesh()
    {
        if (mesh == null)
        {
            return;
        }

        mesh.Clear();

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.Optimize();
    }

    public void DisplayNoiseMap(float[,] noise)
    {
        NoiseDisplay display = FindObjectOfType<NoiseDisplay>();
        display.DrawNoise(noise);
    }

    //private void OnDrawGizmos()
    //{
    //    if (vertices == null)
    //    {
    //        return;
    //    }
    
    //    for (int i = 0; i < vertices.Length; i++)
    //    {
    //        Gizmos.DrawSphere(vertices[i], .1f);
    //    }
    //}
}
