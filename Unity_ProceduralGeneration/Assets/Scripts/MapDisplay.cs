using UnityEngine;
using System.Collections;

public class MapDisplay : MonoBehaviour {

	public Renderer texRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;

	public void DrawTexture(Texture2D texture) {
		texRender.sharedMaterial.mainTexture = texture;
		texRender.transform.localScale = new Vector3 (texture.width, 1, texture.height);
	}

	public void DrawMesh(MeshData meshData) {
		meshFilter.sharedMesh = meshData.CreateMesh ();

		meshFilter.transform.localScale = Vector3.one * FindObjectOfType<MapGenerator> ().terrainData.uniformScale;
	}

}
