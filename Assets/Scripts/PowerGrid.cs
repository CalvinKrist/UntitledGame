using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Untitled.Tiles;
using Untitled.Utils;

namespace Untitled
{
	namespace Power
	{
		public class PowerGridManager
		{
			// Sparse maps to track grid info. Used to determine 
			// if a grid element is on a power grid, and if so,
			// which one.
			private Dictionary<Coords, PowerGrid> posToGridMap;
			private List<PowerGrid> grids;
			
			#region SINGLETON PATTERN
			private static readonly PowerGridManager instance = new PowerGridManager();

			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static PowerGridManager()
			{
			}

			private PowerGridManager()
			{
				posToGridMap = new Dictionary<Coords, PowerGrid>();
				grids = new List<PowerGrid>();
				
				Placeable.OnPlaceableCreateEvent += PlaceableCreatedEventHandler;
				Placeable.OnPlaceableDestroyEvent += PlaceableDestroyedEventHandler;
			}

			public static PowerGridManager Instance{get{return instance;}}
			#endregion
			
			private PowerGrid MergeGrids(PowerGrid g1, PowerGrid g2)
			{
				if(g1 == g2)
					return g1;
				
				PowerGrid smaller;
				PowerGrid larger;
				if(g1.inputs.Count + g1.outputs.Count + g1.cables.Count > g2.inputs.Count + g2.outputs.Count + g2.cables.Count) {
					smaller = g2;
					larger = g1;
				} else {
					smaller = g1;
					larger = g2;
				}
				
				foreach(Building building in smaller.inputs) {
					larger.inputs.Add(building);
					posToGridMap[building.coords] = larger;
				}
				foreach(Building building in smaller.outputs) {
					larger.outputs.Add(building);
					posToGridMap[building.coords] = larger;
				}
				foreach(Cable cable in smaller.cables) {
					larger.cables.Add(cable);
					posToGridMap[cable.coords] = larger;
				}
				
				grids.Remove(smaller);
				return larger;
			}
			
			/***********************
			***  Event Handlers  ***
			************************/
			private void PlaceableCreatedEventHandler(Placeable placeable)
			{
				Coords coords = placeable.coords;
				
				PowerGrid grid = new PowerGrid();
				
				if(placeable.IsBuilding())
					grid.inputs.Add(placeable.GetComponent<Building>());
				else if(placeable.IsCable())
					grid.cables.Add(placeable.GetComponent<Cable>());
				
				grids.Add(grid);
				posToGridMap.Add(coords, grid);
				
				if(posToGridMap.ContainsKey(coords + Vector2Int.up))
				{
					grid = MergeGrids(grid, posToGridMap[coords + Vector2Int.up]);
				}
				if(posToGridMap.ContainsKey(coords + Vector2Int.right))
				{
					grid = MergeGrids(grid, posToGridMap[coords + Vector2Int.right]);
				}
				if(posToGridMap.ContainsKey(coords + Vector2Int.down))
				{
					grid = MergeGrids(grid, posToGridMap[coords + Vector2Int.down]);
				}
				if(posToGridMap.ContainsKey(coords + Vector2Int.left))
				{
					grid = MergeGrids(grid, posToGridMap[coords + Vector2Int.left]);
				}
			}
			
			private void PlaceableDestroyedEventHandler(Placeable placeable)
			{
				
			}
			
		}
		
		public class PowerGrid
		{
			public List<Building> inputs;
			public List<Building> outputs;
			public List<Cable> cables;
			
			public PowerGrid()
			{
				Debug.Log("creating grid");
				inputs = new List<Building>();
				outputs = new List<Building>();
				cables = new List<Cable>();
			}
			
			~PowerGrid()
			{
				Debug.Log("destroying grid");
			}
		}

	}
}