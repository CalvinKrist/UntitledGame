using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Untitled.Resource;
using Untitled.Configs;
using System.Linq;
using Untitled.Utils;

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
                            storageMap[FlatIndex(i, j)] = new ResourceTile(Config.Instance.TileConfig.TileStartingValues[CheckType(tile)]);
                        }
                    }
                }
            }
			
			/*** CheckType ***/
			
			public TileType CheckType(Coords coords)
            {
                return CheckType(tilemap.GetTile<Tile>(
					tilemap.WorldToCell(coords.AsTile())
				));
            }
			
            public TileType CheckType(Tile tile)
            {
                if (tile != null)
                {
                    string asset_name = tile.sprite.name;
                    if (asset_name.ToLower().Contains("coal"))
                        return TileType.Coal;
					else if (asset_name.ToLower().Contains("ground"))
                        return TileType.Ground;
                    return TileType.Other;
                }
                return TileType.None;
            }
            /*** End CheckType ***/
			
			public ResourceTile GetResourceTile(Coords coords)
            {
				Vector3Int pos = tilemap.WorldToCell(coords.AsTile());
                int index = FlatIndex(pos);
				
                ResourceTile tile;
                if (storageMap.TryGetValue(index, out tile)) 
                    return tile;
                
                return null;
            }

            /*** FlatIndex ***/

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
            /*** End FlatIndex ***/

            /*** GetValueAt ***/
            public float GetValueAt(Coords coords)
            {
                Vector3Int pos = tilemap.WorldToCell(coords.AsTile());
                return GetValueAt(pos);
            }

            private float GetValueAt(Vector3Int pos)
            {
                return GetValueAt(FlatIndex(pos));
            }

            private float GetValueAt(int pos)
            {
                ResourceTile tile;
                if (storageMap.TryGetValue(pos, out tile))
                {
                    return tile.GetResourceCount(ResourceType.None);
                } 
                else
                {
                    return 0;
                }
            }
			/*** End GetValueAt ***/

        }
    }
}

