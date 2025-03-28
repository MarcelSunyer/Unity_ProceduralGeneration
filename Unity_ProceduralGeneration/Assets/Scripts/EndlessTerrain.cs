using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    const float scale = 2f;

    const float viewerMoveThresholdforChunkUpdate = 25f;
    const float sqrViewerMoveThresholdforChunkUpdate = viewerMoveThresholdforChunkUpdate * viewerMoveThresholdforChunkUpdate;

    public LODInfo[] detailLevels;
    public static float maxViewDst;

    public Transform viewer;
    public Material mapMaterial;

    public static Vector2 viewerPosition;

    Vector2 viewerPositionOld;
    static MapGenerator mapGenerator;
    int chunkSize;
    int chunkVisibleInViewDst;

    Dictionary<Vector2, TerrainChunk> terrainChinkDictionay = new Dictionary<Vector2, TerrainChunk>();
    static List<TerrainChunk> terrainChunksvVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = FindAnyObjectByType<MapGenerator>();

        maxViewDst = detailLevels[detailLevels.Length - 1].visibleDstThreshold;
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunkVisibleInViewDst = Mathf.RoundToInt(maxViewDst / chunkSize);
        UpdateVisibleChunks();
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z) / scale;

        if((viewerPositionOld - viewerPosition).sqrMagnitude> sqrViewerMoveThresholdforChunkUpdate)
        {
            viewerPositionOld = viewerPosition;
            UpdateVisibleChunks();
        
        }

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
                    terrainChinkDictionay.Add(viewedChunkCoord,new TerrainChunk(viewedChunkCoord,chunkSize, detailLevels, transform, mapMaterial));
                }
                
            }
        }
    }

    public class TerrainChunk
    {
        GameObject meshObject;
        Vector2 position;
        Bounds bounds;

  

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        MeshCollider meshCollider;

        LODInfo[] detailLevels;
        LODMesh[] lodMeshes;
        LODMesh collisionLODMesh;

        MapData mapData;
        bool mapDataReceived;
        int previusLODIndex = -1;
        public TerrainChunk(Vector2 coord, int size, LODInfo[] _detailLevels, Transform parent, Material material)
        {
            this.detailLevels = _detailLevels;
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshRenderer.material = material;
            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;
            SetVisible(false);

            lodMeshes = new LODMesh[detailLevels.Length];
            for (int i = 0; i < detailLevels.Length; ++i)
            {
                lodMeshes[i] = new LODMesh(detailLevels[i].lod, UpdateTerrainChunk);
                if (detailLevels[i].useForCollider)
                {
                    collisionLODMesh = lodMeshes[i];
                }
            }

            mapGenerator.RequestMapData(position, OnMapDataRecive);
        }
        void OnMapDataReciveed(MapData mapData)
        {

        }
        void OnMapDataRecive(MapData mapData)
        {
            this.mapData = mapData;
            mapDataReceived = true;

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap,MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            UpdateTerrainChunk();
        }

        public void UpdateTerrainChunk()
        {
            if (mapDataReceived)
            {

                float viewerDstFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
                bool visible = viewerDstFromNearestEdge <= maxViewDst;

                if (visible)
                {
                    int lodIndex = 0;
                    for (int i = 0; i < detailLevels.Length - 1; ++i)
                    {
                        if (viewerDstFromNearestEdge > detailLevels[i].visibleDstThreshold)
                        {
                            lodIndex = i + 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (lodIndex != previusLODIndex)
                    {
                        LODMesh lodMesh = lodMeshes[lodIndex];
                        if (lodMesh.hasMesh)
                        {
                            previusLODIndex = lodIndex;
                            meshFilter.mesh = lodMesh.mesh;
                            meshCollider.sharedMesh = lodMesh.mesh;
                        }
                        else if (!lodMesh.hasRequestedMesh)
                        {
                            lodMesh.RquestMesh(mapData);
                        }
                    }
                    if(lodIndex ==0)
                    {
                        if(collisionLODMesh.hasMesh)
                        {
                            meshCollider.sharedMesh = collisionLODMesh.mesh;
                        }
                        else if(!collisionLODMesh.hasRequestedMesh)
                        {
                            collisionLODMesh.RquestMesh(mapData);
                        }
                    }
                    terrainChunksvVisibleLastUpdate.Add(this);
                }
                SetVisible(visible);
            }
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
    class LODMesh
    {
        public Mesh mesh;
        public bool hasRequestedMesh;
        public bool hasMesh;
        int lod;
        System.Action updateCallback;

        public LODMesh(int lod, System.Action updateCallback)
        {
            this.lod = lod;
            this.updateCallback = updateCallback;
        }

        void OnMeshDataRecived(MeshData meshData)
        {
            mesh = meshData.CreateMesh();
            hasMesh = true;

            updateCallback();
        }
        public void RquestMesh(MapData mapData)
        {
            hasRequestedMesh = true;
            mapGenerator.RequestMeshData(mapData,lod, OnMeshDataRecived);
        }
    }

    [System.Serializable]
    public struct LODInfo
    {
        public int lod;
        public float visibleDstThreshold;
        public bool useForCollider;
    }
}
