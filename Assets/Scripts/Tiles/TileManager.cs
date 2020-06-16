using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Untitled.Resource;
using Untitled.Configs;
using System.Linq;


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

            private Tile GetTileAt(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = tilemap.WorldToCell(pos);
                return tilemap.GetTile<Tile>(gridCoords);
            }

            /*** CheckType ***/
			
            public TileType CheckType(Vector3 pos)
            {
                return CheckType(GetTileAt(pos));
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
                    return tile.GetResourceCount(ResourceType.None);
                } 
                else
                {
                    return 0;
                }
            }
			
			public ResourceTile GetResourceTileAtWorldCoords(Vector3 worldCoords)
            {
				worldCoords = Camera.main.ScreenToWorldPoint(worldCoords);
                worldCoords.z = 0;
                Vector3Int gridCoords = tilemap.WorldToCell(worldCoords);
				int index = FlatIndex(gridCoords);
				
                ResourceTile tile;
                if (storageMap.TryGetValue(index, out tile)) 
                    return tile;
                
                return null;
            }

            public bool ModifyTileStorage(Vector3 pos, int delta)
            {
                return false;
            }

            public Vector3 CastWorldCoordsToTile(Vector3 pos)
            {
                pos = Camera.main.ScreenToWorldPoint(pos);
                pos.z = 0;
                Vector3Int gridCoords = tilemap.WorldToCell(pos);
                return tilemap.CellToWorld(gridCoords);
            }

        }
    }
}

