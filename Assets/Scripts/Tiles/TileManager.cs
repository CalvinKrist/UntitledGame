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

            void Awake()
            {
                storageMap = new Dictionary<int, ResourceStorage>();
            }

            public TileType CheckType(Vector3 pos)
            {
                Tile t = GetTileAt(pos);
                Debug.Log("My tile: " + t);
                string asset_name = t.sprite.name;
                if (asset_name.ToLower().Contains("coal"))
                    return TileType.Coal;
                return TileType.Other;
            }

            public bool ModifyTileStorage(Vector3 pos, int delta)
            {
                return false;
            }

            private Tile GetTileAt(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Debug.Log("Converted Coords: " + pos);
                return tilemap.GetTile<Tile>(new Vector3Int((int)pos.x, (int)pos.y, (int)pos.z));                
            }
        }
    }
}

