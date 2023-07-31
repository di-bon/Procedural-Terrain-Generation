using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class NoiseDisplay : MonoBehaviour
{
    public Transform terrain;
    public Renderer noiseRenderer;

    [Range(-100, 100)]
    public float yOffset = 0f;

    [Range(-1f, 1f)]
    public float fineYOffset = 0f;

    public Color lowColor;
    public Color highColor;

    [Range(0, 1)]
    public float transparency = .5f;

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Color[] colors;

    private float[,] noise;
    private int xSize;
    private int zSize;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void OnValidate()
    {
        transform.position = new Vector3(terrain.position.x, yOffset + fineYOffset, terrain.position.z);

        SetColor();
        SetTransparency();
        UpdateMesh();
    }

    public void DrawNoise(float[,] noise)
    {
        this.noise = noise;
        xSize = this.noise.GetLength(0) - 1;
        zSize = this.noise.GetLength(1) - 1;
        
        //this.noise = Noise.NormalizeNoise(this.noise);

        CreateMesh(xSize, zSize);
    }

    private void CreateMesh(int xSize, int zSize)
    {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];

        for (int i = 0, z = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[i] = new Vector3(x, yOffset, z);
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

        SetColor();

        SetTransparency();

        UpdateMesh();
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
        mesh.colors = colors;

        mesh.RecalculateNormals();
        mesh.Optimize();
    }

    private void SetColor()
    {
        if (noise == null)
        {
            return;
        }

        colors = new Color[vertices.Length];

        for (int z = 0, i = 0; z <= zSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                colors[i] = Color.Lerp(lowColor, highColor, noise[x, z]);
                i++;
            }
        }
    }


    private void SetTransparency()
    {
        noiseRenderer.sharedMaterial.SetFloat("_Transparency", transparency);
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
