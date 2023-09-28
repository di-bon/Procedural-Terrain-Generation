using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenerateTerrainMesh(float[,] noiseMap, float heightMultiplier, float yOffset)
    {
        int xSize = noiseMap.GetLength(0);
        int zSize = noiseMap.GetLength(1);

        //float topLeftX = (xSize - 1) / -2f;
        //float topLeftZ = (zSize - 1) / 2f;

        MeshData meshData = new MeshData(xSize, zSize);
        int vertexIndex = 0;

        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                //meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, noiseMap[x, z], topLeftZ - z);
                meshData.vertices[vertexIndex] = new Vector3(x, noiseMap[x, z] * heightMultiplier + yOffset, z);

                meshData.uvs[vertexIndex] = new Vector2(x / (float)xSize, z / (float)zSize);

                if (x < xSize - 1 && z < zSize - 1)
                {
                    meshData.AddTriangle(vertexIndex + xSize, vertexIndex + xSize + 1, vertexIndex);
                    meshData.AddTriangle(vertexIndex + 1, vertexIndex, vertexIndex + xSize + 1);
                }

                vertexIndex++;
            }
        }

        return meshData;
    }

    //public static MeshData GenerateNoiseMesh(float[,] noiseMap, float y)
    //{
    //    int xSize = noiseMap.GetLength(0);
    //    int zSize = noiseMap.GetLength(1);

    //    MeshData meshData = new MeshData(xSize, zSize);
    //    int vertexIndex = 0;

    //    for (int z = 0; z < zSize; z++)
    //    {
    //        for (int x = 0; x < xSize; x++)
    //        {
    //            meshData.vertices[vertexIndex] = new Vector3(x, y, z);

    //            meshData.uvs[vertexIndex] = new Vector2(x / (float)xSize, z / (float)zSize);

    //            if (x < xSize - 1 && z < zSize - 1)
    //            {
    //                meshData.AddTriangle(vertexIndex + xSize, vertexIndex + xSize + 1, vertexIndex);
    //                meshData.AddTriangle(vertexIndex + 1, vertexIndex, vertexIndex + xSize + 1);
    //            }

    //            vertexIndex++;
    //        }
    //    }

    //    return meshData;
    //}
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;
    public Color[] colors;

    int triangleIndex = 0;

    public MeshData(int xSize, int zSize)
    {
        vertices = new Vector3[xSize * zSize];
        uvs = new Vector2[vertices.Length];
        triangles = new int[(xSize - 1) * (zSize - 1) * 6];
        colors = new Color[vertices.Length];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public void SetColor(int index, Color color)
    {
        colors[index] = color;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.colors = colors;
        mesh.RecalculateNormals();
        mesh.Optimize();

        return mesh;
    }
}
