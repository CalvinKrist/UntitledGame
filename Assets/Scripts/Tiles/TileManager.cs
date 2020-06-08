using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Untitled.Resource;
using Untitled.Configs;
using System.Linq;
using JetBrains.Annotations;
using System.Dynamic;

namespace Untitled
{
    namespace Tiles
    {
        public class TileManager : MonoBehaviour
        {
            public Dictionary<ResourceType, int> worldStartingResources;
            public Tilemap tilemap;

            private Dictionary<int, ResourceTile> storageMap;

            void Awake()
            {
                storageMap = new Dictionary<int, ResourceTile>();
            }

            void Start()
            {
                for (int i = tilemap.cellBounds.xMin; i < tilemap.cellBounds.xMax; i++)
                {
                    for (int j = tilemap.cellBounds.yMin; j < tilemap.cellBounds.yMax; j++)
                    {
                        Tile tile = tilemap.GetTile<Tile>(new Vector3Int(i, j, 0));

                        if (tile == null)
                            continue;

                        TileType tileType = CheckType(tile);

                        if (Config.Instance.TileConfig.ResourceTiles.Contains(tileType))
                        {
                            storageMap[FlatIndex(new Vector2Int(i, j))] = new ResourceTile(Config.Instance.TileConfig.TileStartingValues[CheckType(tile)]);
                        }
                    }
                }
            }

            private int FlatIndex(Vector3Int pos)
            {
                return FlatIndex(pos.x, pos.y);
            }

            private int FlatIndex(Vector2Int pos)
            {
                return FlatIndex(pos.x, pos.y);
            }

            private int FlatIndex(int x, int y)
            {
                return x * tilemap.cellBounds.y + y;
            }

            public TileType CheckType(Vector3 pos)
            {
                return CheckType(GetTileAt(pos));
            }

            public float GetValueAt(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = tilemap.WorldToCell(pos);
                return GetValueAt(gridCoords);
            }

            public float GetValueAt(Vector3Int pos)
            {
                return GetValueAt(FlatIndex(pos));
            }

            public float GetValueAt(int pos)
            {
                ResourceTile tile;
                if (storageMap.TryGetValue(pos, out tile))
                {
                    return tile.Value;
                } 
                else
                {
                    return 0;
                }
            }

            public TileType CheckType(Tile tile)
            {
                if (tile != null)
                {
                    string asset_name = tile.sprite.name;
                    if (asset_name.ToLower().Contains("coal"))
                        return TileType.Coal;
                    return TileType.Other;
                }
                return TileType.None;
            }

            public bool ModifyTileStorage(Vector3 pos, int delta)
            {
                return false;
            }

            private Tile GetTileAt(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = tilemap.WorldToCell(pos);
                Debug.Log("Converted Coords: " + gridCoords);
                return tilemap.GetTile<Tile>(gridCoords);                
            }
        }
    }
}

