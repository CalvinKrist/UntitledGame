using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Untitled.Tiles;

namespace Untitled
{
	namespace Power
	{
		public class PowerGridManager
		{
			
			private Vector3 dx = new Vector3(0.5f, 0, 0);
			private Vector3 dy = new Vector3(0, 0.25f, 0);
			
			// Sparse maps to track grid info
			private Dictionary<Vector3, PowerGrid> posToGridMap;
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
				posToGridMap = new Dictionary<Vector3, PowerGrid>();
				grids = new List<PowerGrid>();
				
				Building.OnBuildingCreateEvent += BuildingCreatedEventHandler;
				Building.OnBuildingDestroyEvent += BuildingDestroyedEventHandler;
			}

			public static PowerGridManager Instance{get{return instance;}}
			#endregion
			
			private PowerGrid MergeGrids(PowerGrid g1, PowerGrid g2)
			{
				if(g1 == g2)
					return g1;
				
				PowerGrid smaller;
				PowerGrid larger;
				if(g1.inputs.Count + g1.outputs.Count > g2.inputs.Count + g2.outputs.Count) {
					smaller = g2;
					larger = g1;
				} else {
					smaller = g1;
					larger = g2;
				}
				
				foreach(Building building in smaller.inputs) {
					larger.inputs.Add(building);
					Vector3 gridCoords = building.gameObject.transform.position;
					posToGridMap[gridCoords] = larger;
				}
				foreach(Building building in smaller.outputs) {
					larger.outputs.Add(building);
					Vector3 gridCoords = building.gameObject.transform.position;
					posToGridMap[gridCoords] = larger;
				}
				
				grids.Remove(smaller);
				return larger;
			}
			
			/***********************
			***  Event Handlers  ***
			************************/
			private void BuildingCreatedEventHandler(Building building)
			{
				Vector3 gridCoords = building.gameObject.transform.position;
				
				PowerGrid grid = new PowerGrid();
				grid.inputs.Add(building);
				grids.Add(grid);
				posToGridMap.Add(gridCoords, grid);
				
				if(posToGridMap.ContainsKey(gridCoords + dx + dy))
				{
					grid = MergeGrids(grid, posToGridMap[gridCoords + dx + dy]);
				}
				if(posToGridMap.ContainsKey(gridCoords + dx - dy))
				{
					grid = MergeGrids(grid, posToGridMap[gridCoords + dx - dy]);
				}
				if(posToGridMap.ContainsKey(gridCoords - dx + dy))
				{
					grid = MergeGrids(grid, posToGridMap[gridCoords - dx + dy]);
				}
				if(posToGridMap.ContainsKey(gridCoords - dx - dy))
				{
					grid = MergeGrids(grid, posToGridMap[gridCoords - dx - dy]);
				}
			}
			
			private void BuildingDestroyedEventHandler(Building building)
			{
				
			}
			
		}
		
		public class PowerGrid
		{
			public List<Building> inputs;
			public List<Building> outputs;
			
			public PowerGrid()
			{
				Debug.Log("creating grid");
				inputs = new List<Building>();
				outputs = new List<Building>();
			}
			
			~PowerGrid()
			{
				Debug.Log("destroying grid");
			}
		}

	}
}