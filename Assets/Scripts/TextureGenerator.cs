using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromColorMap(Color[] colorMap, int xSize, int zSize)
    {
        Texture2D texture = new Texture2D(xSize, zSize);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colorMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromNoiseMap(float[,] noiseMap)
    {
        int xSize = noiseMap.GetLength(0);
        int zSize = noiseMap.GetLength(1);

        Color[] colorMap = new Color[xSize * zSize];
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                colorMap[z * xSize + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, z]);
            }
        }

        return TextureFromColorMap(colorMap, xSize, zSize);
    }
}
