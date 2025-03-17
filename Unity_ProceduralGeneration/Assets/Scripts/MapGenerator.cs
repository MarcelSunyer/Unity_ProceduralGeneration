using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColourMap
    }
    public DrawMode drawMode;

    public int mapWidth;
    public int mapHeigth;
    public float noisScale;

    public int octaves;
    [Range(0,1)]

    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool autoUpdate;

    public TerrainTypes[] regions;
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeigth, seed, noisScale, octaves, persistance, lacunarity, offset);

        Color[] colourMap = new Color[mapWidth * mapHeigth];
        for (int y = 0; y < mapHeigth; ++y)
        {
            for (int x = 0; x < mapWidth; ++x)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if(currentHeight <= regions[i].height)
                    {
                        colourMap[y * mapWidth + x] = regions[i].color;
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        }else if(drawMode == DrawMode.ColourMap)
        {
            display.DrawTexture(TextureGenerator.TextureFromColourMap(colourMap,mapWidth,mapHeigth));
        }
    }
    private void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeigth < 1)
        {
            mapHeigth = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }
        if(octaves< 0)
        {
            octaves = 0;
        }
    }
}

[System.Serializable]
public struct TerrainTypes
{
    public string name;
    public float height;
    public Color color;

}