using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;

namespace Untitled
{
	namespace Utils
	{
		/*
		* Provides ways to convert "tile coordinates" to
		* cartesian grid coordinates, and then query world
		* information based on those grid coordinates.
		*
		* This is useful to, for example, figure out what's
		* surrounding a tile.
		*/
		public class GridUtils : MonoBehaviour
		{
			public TileManager tileManager;
			
			public readonly Vector3 dx = new Vector3(0.5f, 0, 0);
			public readonly Vector2 dy = new Vector3(0, 0.25f, 0);
			
			#region SINGLETON PATTERN
			private static GridUtils _instance;
			public static GridUtils Instance
			{
			 get {
				 if (_instance == null)
				 {
					 _instance = GameObject.FindObjectOfType<GridUtils>();
					 
					 if (_instance == null)
					 {
						 GameObject container = new GameObject("GameController");
						 _instance = container.AddComponent<GridUtils>();
					 }
				 }
			 
				 return _instance;
			 }
			}
			#endregion
			
			private Dictionary<Coords, Placeable> placeableMap;
			
			void Start()
			{
				placeableMap = new Dictionary<Coords, Placeable>();
				
				Placeable.OnPlaceableCreateEvent += (Placeable placeable) => {
					foreach(Coords coords in placeable.GetBounds()) {
						placeableMap[coords] = placeable;
					}
				};
				Placeable.OnPlaceableDestroyEvent += (Placeable placeable) => {
					foreach(Coords coords in placeable.GetBounds())
						placeableMap.Remove(coords);
				};
			}
			
			public static Placeable GetPlaceableAt(Coords coords)
			{
				if(Instance.placeableMap.ContainsKey(coords))
					return Instance.placeableMap[coords];
				return null;
			}
			
			public static TileType GetTileTypeAt(Coords coords)
			{
				return Instance.tileManager.CheckType(coords);
			}
			
			public static ResourceTile GetResourceTileAt(Coords coords)
			{
				return Instance.tileManager.GetResourceTile(coords);
			}
			
			// Casts world coordinates to a 'Coords' object.
			// World coords are things like raw mouse position.
			public static Coords WorldToCoords(Vector3 worldCoords)
			{
				// Convert world coords to tile coords through
				// camera transformations.
				Vector3 pos = Camera.main.ScreenToWorldPoint(worldCoords);
                pos.z = 0;
				
				// Round tile coords to center of the nearest 
				// tile by converting back and forth
                Vector3Int gridCoords = Instance.tileManager.tilemap.WorldToCell(pos);
                Vector3 coords = Instance.tileManager.tilemap.CellToWorld(gridCoords);
				
				return new Coords(coords);
			}
			
			public static void TestGetGridCoords()
			{
				// Maps 'counts' to expected output
				Dictionary<Vector2Int, Vector2Int> testCases = new Dictionary<Vector2Int, Vector2Int>();
				
				testCases[new Vector2Int(-4, 0)] = new Vector2Int(-2, 2);
				testCases[new Vector2Int(-3, 1)] = new Vector2Int(-1, 2);
				testCases[new Vector2Int(-2, 2)] = new Vector2Int(0, 2);
				testCases[new Vector2Int(-1, 3)] = new Vector2Int(1, 2);
				testCases[new Vector2Int(0, 4)] = new Vector2Int(2, 2);
				testCases[new Vector2Int(-4, 0)] = new Vector2Int(-2, 2);
				testCases[new Vector2Int(-3, -1)] = new Vector2Int(-2, 1);
				testCases[new Vector2Int(-2, 0)] = new Vector2Int(-1, 1);
				testCases[new Vector2Int(-1, 1)] = new Vector2Int(0, 1);
				testCases[new Vector2Int(0, 2)] = new Vector2Int(1, 1);
				testCases[new Vector2Int(1, 3)] = new Vector2Int(2, 1);testCases[new Vector2Int(-2, -2)] = new Vector2Int(-2, 0);
				testCases[new Vector2Int(-1, -1)] = new Vector2Int(-1, 0);
				testCases[new Vector2Int(1, 1)] = new Vector2Int(1, 0);
				testCases[new Vector2Int(2, 2)] = new Vector2Int(2, 0);
				testCases[new Vector2Int(-1, -3)] = new Vector2Int(-2, -1);
				testCases[new Vector2Int(-0, -2)] = new Vector2Int(-1, -1);
				testCases[new Vector2Int(1, -1)] = new Vector2Int(0, -1);
				testCases[new Vector2Int(2, 0)] = new Vector2Int(1, -1);
				testCases[new Vector2Int(3, 1)] = new Vector2Int(2, -1);
				testCases[new Vector2Int(0, -4)] = new Vector2Int(-2, -2);
				testCases[new Vector2Int(1, -3)] = new Vector2Int(-1, -2);
				testCases[new Vector2Int(2, -2)] = new Vector2Int(0, -2);
				testCases[new Vector2Int(3, -1)] = new Vector2Int(1, -2);
				testCases[new Vector2Int(4, 0)] = new Vector2Int(2, -2);
				
				var dx = Instance.dx.x;
				var dy = Instance.dy.y;
				int incorrectCount = 0;
				foreach(KeyValuePair<Vector2Int, Vector2Int> entry in testCases)
				{
					Vector3 tileCoords = new Vector3(entry.Key.x * dx, entry.Key.y * dy, 0);
					
					Coords coords = new Coords(tileCoords);
					
					if(coords.AsGrid() == entry.Value)
						Debug.Log("PASSED " + entry.Value);
					else {
						Debug.Log("FAILED:");
						Debug.Log("\ttest: " + entry.Key);
						Debug.Log("\texpected: " + entry.Value);
						Debug.Log("\tactual: " + coords.AsGrid());
						incorrectCount++;
					}
				}
				
				Debug.Log("Test Cases Incorrect: " + incorrectCount);
			}
			
		}
	}
}
