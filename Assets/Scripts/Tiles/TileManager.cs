using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Untitled.Resource;

namespace Untitled
{
    namespace Tiles
    {
        public enum TileType
        {
            Coal,
            Other,
        }
        public class TileManager : MonoBehaviour
        {
            [SerializeField]
            public Tilemap tilemap;
            private Dictionary<int, ResourceStorage> storageMap;
            private Dictionary<string, TileType> typeMap;
            Grid grid;

            void Awake()
            {
                storageMap = new Dictionary<int, ResourceStorage>();
                grid = FindObjectOfType<Grid>();
            }

            public TileType CheckType(Vector3 pos)
            {
                Tile t = GetTileAt(pos);
                Debug.Log("My tile: " + t);
                if (t != null)
                {
                    string asset_name = t.sprite.name;
                    if (asset_name.ToLower().Contains("coal"))
                        return TileType.Coal;
                }
                
                return TileType.Other;
            }

            public bool ModifyTileStorage(Vector3 pos, int delta)
            {
                return false;
            }

            public Tile GetTileAt(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = grid.WorldToCell(pos);
                Debug.Log("Converted Coords: " + gridCoords);
                return tilemap.GetTile<Tile>(gridCoords);                
            }

            public Vector3 CastWorldCoordsToTile(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = grid.WorldToCell(pos);
                return grid.CellToWorld(gridCoords);
            }

        }
    }
}

