using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class NoiseDisplay : MonoBehaviour
{
    public Transform terrain;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    [Range(-100, 100)]
    public float yOffset = 0f;

    [Range(-1f, 1f)]
    public float fineYOffset = 0f;

    public Color lowColor;
    public Color highColor;

    [Range(0, 1)]
    public float transparency = .5f;

    private float[,] noise;
    private Color[] colors;

    public void DrawNoise(float[,] noise)
    {
        this.noise = noise;
        int xSize = noise.GetLength(0);
        int zSize = noise.GetLength(1);

        MeshData meshData = MeshGenerator.GenerateFlatMesh(noise, yOffset + fineYOffset);

        colors = new Color[meshData.vertices.Length];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                colors[z * xSize + x] = Color.Lerp(lowColor, highColor, noise[x, z]);
            }
        }
        meshData.colors = colors;
        Texture2D texture = TextureGenerator.TextureFromNoiseMap(noise);

        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;

        meshRenderer.sharedMaterial.SetTexture("_Noise_Texture", texture);

        SetTransparency();
    }

    private void OnValidate()
    {
        if (noise != null)
        {
            DrawNoise(noise);
        }
    }

    private void SetTransparency()
    {
        meshRenderer.sharedMaterial.SetFloat("_Transparency", transparency);
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
