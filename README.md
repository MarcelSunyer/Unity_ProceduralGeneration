# 🌍 Procedural Terrain Generation in Unity

This Unity project implements **endless terrain generation** using procedural techniques. It dynamically creates mesh-based terrain in chunks and adjusts level of detail (LOD) based on distance from the viewer, ensuring high performance and visual quality.

## 🚀 Features

- ✅ **Endless Terrain Streaming** using player position  
- 🌄 **Procedurally Generated Meshes** based on noise height maps  
- 🧠 **LOD Optimization** for efficient rendering  
- 🖼️ **Heightmap to Texture Conversion**  
- 🎨 **Flat & Smooth Shading Options**

---

## 📁 Project Structure

| Script | Role |
|--------|------|
| `EndlessTerrain.cs` | Manages terrain chunks, updates based on viewer position |
| `MeshGenerator.cs` | Generates terrain meshes based on a heightmap |
| `TextureGenerator.cs` | Converts heightmaps into grayscale textures for visualization |

---

## ⚙️ How It Works

### 🧩 1. Chunk-Based Terrain

`EndlessTerrain.cs` breaks the world into manageable square **chunks** of mesh. As the viewer moves, it loads and unloads these chunks depending on distance, creating the illusion of an infinite world.

> Each chunk includes a **mesh**, **LOD system**, and optional collider data.

---

### ⛰️ 2. Mesh Generation

`MeshGenerator.cs` builds the actual mesh geometry. It converts a 2D `float[,]` heightmap into a Unity `Mesh`:

- Uses a configurable **height multiplier** and **AnimationCurve** to shape the terrain
- Supports **multiple LOD levels**
- Generates both **smooth** and **flat-shaded** terrain

```csharp
MeshData meshData = MeshGenerator.GenerateTerrainMesh(
    heightMap, heightMultiplier, heightCurve, lod, useFlatShading
);
```

---

### 🖌️ 3. Texture Generation

`TextureGenerator.cs` converts heightmaps into grayscale textures, useful for debugging and heightmap visualization.

```csharp
Texture2D texture = TextureGenerator.TextureFromHeightMap(heightMap);
```

---

## 🧪 Customization Tips

- Modify the `heightMultiplier` and `heightCurve` in the inspector to shape your terrain.
- Experiment with different `supportedChunkSizes` and LODs in `MeshGenerator.cs`.
- Use your own noise algorithms to supply the `heightMap`.

---

## 🎮 Getting Started

1. Clone the repository
2. Open the Unity project
3. Attach `EndlessTerrain.cs` to an empty GameObject
4. Assign your **viewer** (e.g. the player camera)
5. Run the scene and explore the endless world!

---

## 📷 Preview

> *Insert a screenshot or GIF of your terrain in action here!*

---

## 📄 License

MIT License – Free to use, modify, and distribute.
