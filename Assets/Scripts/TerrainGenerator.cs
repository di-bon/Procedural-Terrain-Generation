using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
//[System.Serializable]
public class TerrainGenerator : MonoBehaviour {
    private float[,] noiseMap;
    private Color[] colorMap;

    private const int MAX_X = 1000;
    private const int MAX_Z = 1000;

    private TerrainDisplay terrainDisplay;
    private NoiseDisplay noiseDisplay;
    private TerrainDisplay2D terrainDisplay2D;

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

    [Range(1, 10)]
    public int octaves = 4;

    [Range(1f, 10f)]
    public float lacunarity = 3.81f;

    [Range(.0001f, 1f)]
    public float persistance = .593f;

    [Range(1f, 50f)]
    public float heightMultiplier = 1f;

    public TerrainType[] terrainTypes;

    private void Awake()
    {
        terrainDisplay = FindObjectOfType<TerrainDisplay>();
        noiseDisplay = FindObjectOfType<NoiseDisplay>();
        terrainDisplay2D = FindObjectOfType<TerrainDisplay2D>();
    }

    void Start()
    {
        GenerateMap();
        DisplayNoiseMap(noiseMap);
        DisplayTerrain2D(colorMap);
    }

    private void OnValidate()
    {
        if (terrainDisplay != null)
        {
            GenerateMap();
        }

        if (noiseDisplay != null)
        {
            DisplayNoiseMap(noiseMap);
        }

        //if (terrainDisplay2D != null)
        //{
        //    DisplayTerrain2D(colorMap);
        //}
    }

    public void GenerateMap()
    {
        noiseMap = Noise.GenerateNoiseMap(seed, offset, xSize, zSize, scale, frequency, octaves, persistance, lacunarity);

        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, heightMultiplier, 0);
        colorMap = new Color[xSize * zSize];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                float currentHeight = noiseMap[x, z];
                for (int i = 0; i < terrainTypes.Length; i++)
                {
                    if (currentHeight <= terrainTypes[i].height)
                    {
                        colorMap[z * xSize + x] = terrainTypes[i].color;
                        break;
                    }
                }
            }
        }
        Texture2D meshTexture = TextureGenerator.TextureFromColorMap(colorMap, xSize, zSize);
        terrainDisplay.DrawMesh(meshData, meshTexture);
    }

    //private void CreateShape()
    //{
    //    vertices = new Vector3[(xSize + 1) * (zSize + 1)];
    //    uvs = new Vector2[vertices.Length];

    //    noiseMap = Noise.GenerateNoiseMap(seed, offset, xSize, zSize, scale, frequency, amplitude, octaves, persistance, lacunarity, baseMultiplier, exponent);

    //    for (int i = 0, z = 0; z <= zSize; z++)
    //    {
    //        for (int x = 0; x <= xSize; x++)
    //        {
    //            float y = noiseMap[x, z];
    //            vertices[i] = new Vector3(x, y * heightMultiplier, z);
    //            uvs[i] = new Vector2(x / (float)xSize, z / (float)zSize);
    //            i++;
    //        }
    //    }

    //    triangles = new int[xSize * zSize * 6];     // 6 vertices (i.e. 2 triangles) for each square

    //    for (int tris = 0, vert = 0, z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            triangles[tris + 0] = vert + 0;
    //            triangles[tris + 1] = vert + xSize + 1;
    //            triangles[tris + 2] = vert + 1;
    //            triangles[tris + 3] = vert + xSize + 1;
    //            triangles[tris + 4] = vert + xSize + 2;
    //            triangles[tris + 5] = vert + 1;

    //            vert++;
    //            tris += 6;
    //        }
    //        vert++;
    //    }

    //    colors = new Color[vertices.Length];

    //    for (int i = 0, z = 0; z <= zSize; z++)
    //    {
    //        for (int x = 0; x <= xSize; x++)
    //        {
    //            float currentHeight = noiseMap[x, z];
    //            for (int j = 0; j < terrainTypes.Length; j++)
    //            {
    //                if (currentHeight <= terrainTypes[j].height)
    //                {
    //                    colors[z * xSize + x] = terrainTypes[j].color;
    //                    break;
    //                }
    //            }
    //            i++;
    //        }
    //    }
    //}

    //private void UpdateMesh()
    //{
    //    if (mesh == null)
    //    {
    //        return;
    //    }

    //    mesh.Clear();

    //    mesh.vertices = vertices;
    //    mesh.triangles = triangles;
    //    mesh.uv = uvs;
    //    Texture2D meshTexture = TextureGenerator.TextureFromColorMap(colors, xSize, zSize);
    //    meshRenderer.sharedMaterial.mainTexture = meshTexture;

    //    meshRenderer.sharedMaterial.SetTexture("_Terrain_Texture", meshTexture);

    //    //mesh.colors = colors;

    //    mesh.RecalculateNormals();
    //    mesh.Optimize();
    //}

    public void DisplayNoiseMap(float[,] noise)
    {
        noiseDisplay.DrawNoise(noise);
    }

    public void DisplayTerrain2D(Color[] colorMap)
    {
        MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, 0, 0);
        Texture2D texture = TextureGenerator.TextureFromColorMap(colorMap, xSize, zSize);
        terrainDisplay2D.DrawTerrain2D(meshData, texture);
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

[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
