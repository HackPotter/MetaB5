using UnityEngine;

public class CustomTerrainCollider : MonoBehaviour
{
    public TerrainData terrainData;

    public double AlphaCutoff = 0.5f;

    void Update()
    {
        Terrain.activeTerrain.basemapDistance = 100000;
    }
}
