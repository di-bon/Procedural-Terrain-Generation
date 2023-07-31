using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    // offset is a Vector3 instead of a Vector2 in order to use the 'z' field to avoid mistakes
    public static float[,] GenerateNoiseMap(int seed, Vector3 offset, int xSize, int zSize, float scale, float frequency, float amplitude, int octaves, float persistance, float lacunarity, float baseMultiplier, float exponent)
    {
        float[,] noiseMap = new float[xSize + 1, zSize + 1];

        System.Random random = new System.Random(seed);
        Vector3[] octaveOffsets = new Vector3[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = random.Next(-100000, 100000) + offset.x;
            float offsetZ = random.Next(-100000, 100000) + offset.z;
            octaveOffsets[i] = new Vector3(offsetX, 0, offsetZ);
        }

        if (scale <= 0)
        {
            scale = 1;
        }

        float halfXSize = xSize / 2f;
        float halfZSize = zSize / 2f;

        for (int x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                float noiseHeight = 0;
                float amplitudeTmp = amplitude;
                float frequencyTmp = frequency;

                //float amplitudeSum = 0f;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfXSize) / scale * frequencyTmp + octaveOffsets[i].x;
                    float sampleZ = (z - halfZSize) / scale * frequencyTmp + octaveOffsets[i].z;
                    float perlinValue = (Mathf.PerlinNoise(sampleX, sampleZ) * 2 - 1) * amplitudeTmp;
                    noiseHeight += perlinValue;

                    //amplitudeSum += amplitudeTmp;

                    amplitudeTmp *= persistance;
                    frequencyTmp *= lacunarity;
                }

                //noiseHeight /= amplitudeSum;

                noiseMap[x, z] = noiseHeight;
            }
        }

        noiseMap = NormalizeNoise(noiseMap);

        for (int x = 0; x <= xSize; x++)
        {
            for (int z = 0; z <= zSize; z++)
            {
                noiseMap[x, z] = Mathf.Pow(noiseMap[x, z] * baseMultiplier, exponent);
            }
        }

        noiseMap = NormalizeNoise(noiseMap);

        return noiseMap;
    }

    public static float[,] NormalizeNoise(float[,] noiseMap)
    {
        float xSize = noiseMap.GetLength(0);
        float zSize = noiseMap.GetLength(1);

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                if (noiseMap[x, z] > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseMap[x, z];
                }
                if (noiseMap[x, z] < minNoiseHeight)
                {
                    minNoiseHeight = noiseMap[x, z];
                }
            }
        }

        // normalization
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                noiseMap[x, z] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, z]);
            }
        }

        return noiseMap;
    }
}
