using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    public static Texture2D TextureFromColourMap(Color[] colourMap, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.SetPixels(colourMap);
        texture.Apply();
        return texture;
    }

    public static Texture2D TextureFromHeightMap(float[,] heightMap)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(0);

        Color[] colourMap = new Color[width * height];
        for (int i = 0; i < height; i++)
        {
            for (int x = 0; x < width; x++)
            {
                colourMap[i * width + x] = Color.Lerp(Color.black, Color.white, heightMap[x, i]);
            }
        }

        return TextureFromColourMap(colourMap, width, height);

    }

}
