using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{

    private Dictionary<ResurceTile, ResourceStorage> storageMap;

    void Awake()
    {
        storageMap = new Dictionary<Tile, ResourceStorage>();
    }

    void RegisterResourceTile(ResurceTile tile)
    {
        storageMap[tile] = new ResourceStorage();
    }

    void UpdateResourceStorage(ResurceTile tile, ResourceType type, int delta)
    {
        storageMap[tile].AddResources(type, delta);
    }
}
