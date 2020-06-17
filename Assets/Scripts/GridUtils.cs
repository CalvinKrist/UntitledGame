using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;

namespace Untitled
{
	namespace Utils
	{
		public class GridUtils : MonoBehaviour
		{
			public TileManager tileManager;
			
			public readonly Vector3 dx = new Vector3(0.5f, 0, 0);
			public readonly Vector2 dy = new Vector3(0, 0.25f, 0);
			
			#region SINGLETON PATTERN
			private static GridUtils _instance;
			private static GridUtils Instance
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
			
			/*
			* Given world coordinates -- the actual coordinates of an 
			* object, often found by gamneObject.transform.position --
			* return the "grid coords".
			*
			* Grid coords could be viewed as the cartesian plane coordinates
			* if the isometric grid was mapped to a normal square grid and 
			* viewed from above.
			*/
			public static Vector2Int GetGridCoords(Vector3 worldCoords)
			{
				var dx = Instance.dx;
				var dy = Instance.dy;
				
				float xCount = worldCoords.x / dx.x;
				float yCount = worldCoords.y / dy.y;
								
				bool roundedX = false;
				if(xCount % 2 != 0) {
					roundedX = true;
					xCount = xCount > 0 ? xCount + 1 : xCount - 1;
				}
				bool roundedY = false;
				if(yCount % 2 != 0) {
					roundedY = true;
					yCount = yCount > 0 ? yCount + 1 : yCount - 1;
				}
				
				// Create initial x and y based on rounding
				// by tracing across the major diagonal
				int x = (int)(xCount / 2);
				int y = x;
				if(y <= x)
					y *=  -1;
								
				// Correct by tracing back across the minor 
				// diagonal
				int correction = (int)(yCount / 2);
				x += correction;
				y += correction;
								
				// Account for rounding
				if(correction > 0 && roundedX) {
					if(xCount > 0)
						x -= 1;
					else
						y -= 1;
				}
				else if(correction < 0 && roundedY) {
					if(xCount > 0)
						y += 1;
					else
						x += 1;
				}
								
				return new Vector2Int(x, y);
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
					Vector3 coords = new Vector3(entry.Key.x * dx, entry.Key.y * dy, 0);
					
					Vector2Int result = GetGridCoords(coords);
					
					if(result == entry.Value)
						Debug.Log("PASSED " + entry.Value);
					else {
						Debug.Log("FAILED:");
						Debug.Log("\ttest: " + entry.Key);
						Debug.Log("\texpected: " + entry.Value);
						Debug.Log("\tactual: " + result);
						incorrectCount++;
					}
				}
				
				Debug.Log("Test Cases Incorrect: " + incorrectCount);
			}
		}
	}
}
