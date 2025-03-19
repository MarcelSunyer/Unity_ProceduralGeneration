using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDst = 450;
    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunkVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChinkDictionay = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksvVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = FindAnyObjectByType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }
    void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksvVisibleLastUpdate.Count; i++)
        {
            terrainChunksvVisibleLastUpdate[i].SetVisible(false);
        }


        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunkVisibleInViewDst; yOffset <= chunkVisibleInViewDst; ++yOffset)
        {
            for (int xOffset = -chunkVisibleInViewDst; xOffset <= chunkVisibleInViewDst; ++xOffset)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(terrainChinkDictionay.ContainsKey(viewedChunkCoord))
                {
                    terrainChinkDictionay[viewedChunkCoord].UpdateTerrainChunk();
                    if (terrainChinkDictionay[viewedChunkCoord].IsVisilbe())
                    {
                        terrainChunksvVisibleLastUpdate.Add(terrainChinkDictionay[viewedChunkCoord]);
                    }
                }
                else
                {
                    terrainChinkDictionay.Add(viewedChunkCoord,new TerrainChunk(viewedChunkCoord,chunkSize, transform, mapMaterial));
                }
                
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

        MapData mapData;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;
            meshObject.transform.position = positionV3;
            meshObject.transform.parent = parent;
            SetVisible(false);

            mapGenerator.RequestMapData(OnMapDataRecive);
        }

        void OnMapDataRecive(MapData mapData)
        {
            mapGenerator.RequestMeshData(mapData, OnMeshDataRecive);
        }

        void OnMeshDataRecive(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }
        public void UpdateTerrainChunk()
        {
            float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDstFromNearestEdge <= maxViewDst;
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisilbe()
        {
            return meshObject.activeSelf;
        }

    }
}
