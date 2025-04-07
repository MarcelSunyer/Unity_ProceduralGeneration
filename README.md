# 🌍 Procedural Terrain Generation in Unity

This Unity project implements **endless terrain generation** using procedural techniques. It dynamically creates mesh-based terrain in chunks and adjusts level of detail (LOD) based on distance from the viewer, ensuring high performance and visual quality.

## 🚀 Features

- ✅ **Endless Terrain Streaming** using player position  
- 🌄 **Procedurally Generated Meshes** based on noise height maps  
- 🧠 **LOD Optimization** for efficient rendering  
- 🖼️ **Heightmap to Texture Conversion**  
- 🎨 **Flat & Smooth Shading Options**

---

## 🧪 Customization Tips

- Modify the `heightMultiplier` and `heightCurve` in the inspector to shape your terrain.
- Experiment with different `supportedChunkSizes` and LODs in `MeshGenerator.cs`.
- Use your own noise algorithms to supply the `heightMap`.
- The map can also be modified by adjusting the mesh, allowing for tests to observe how noise affects the terrain.

[https://github.com/user-attachments/assets/96e611ba-7090-4905-804c-c887a872ba3d](https://github.com/user-attachments/assets/8206a014-8ee2-45d6-8708-88c2b2786a55)

---

## 🎮 Getting Started

1. Clone the repository
2. Open the Unity project
4. Assign your **viewer** (e.g. the player camera)
5. Run the scene and explore the endless world!

---

## 📷 Preview

[https://github.com/user-attachments/assets/5d3b820e-6fbd-48c6-98de-93129982da91](https://github.com/user-attachments/assets/6d42c5f0-480a-4372-92cc-4b1ee7cdb0f1)

---

## 📄 License

MIT License – Free to use, modify, and distribute.

Huge inspiration and code structure based on the brilliant videos by **[Sebastian Lague](https://www.youtube.com/@SebastianLague)**.  

