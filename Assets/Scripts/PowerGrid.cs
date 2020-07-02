using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Untitled.Tiles;
using Untitled.Utils;
using Untitled.Resource;

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
				
				Placeable.OnPlaceableCreateEventP1 += PlaceableCreatedEventHandler;
				Placeable.OnPlaceableDestroyEventP1 += PlaceableDestroyedEventHandler;
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
					
					foreach(Coords coords in building.GetBounds())
						posToGridMap[coords] = larger;
				}
				foreach(Building building in smaller.outputs) {
					larger.outputs.Add(building);
					
					foreach(Coords coords in building.GetBounds())
						posToGridMap[coords] = larger;
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

				// Create a PowerGrid of just this placeable
				PowerGrid grid = new PowerGrid();
				
				grid.Add(placeable);
				
				grids.Add(grid);
				foreach(Coords coords in placeable.GetBounds())
					posToGridMap.Add(coords, grid);
				
				// Look through all surrounding tiles. If there's
				// a pre-existing grid there, merge then together.
				List<Coords> surroundingTiles = placeable.GetSurroundingTiles();
				foreach(Coords coords in surroundingTiles)
				{
					if(posToGridMap.ContainsKey(coords))
						grid = MergeGrids(grid, posToGridMap[coords]);
				}
			}
			
			private void PlaceableDestroyedEventHandler(Placeable placeable)
			{
				PowerGrid oldGrid = posToGridMap[placeable.coords];
				
				// Update existing data structures with the removal
				grids.Remove(oldGrid);
				foreach(Coords coords in placeable.GetBounds())
					posToGridMap.Remove(coords);
				
				// Remove this placeable from the old grid
				if(placeable.IsBuilding())
				{
					Building building = placeable.GetComponent<Building>();
					oldGrid.inputs.Remove(building);
					oldGrid.outputs.Remove(building);
				}
				if(placeable.IsCable())
				{
					Cable cable = placeable.GetComponent<Cable>();
					oldGrid.cables.Remove(cable);
				}
				
				// For each surrounding tile, find which parts of the 
				// old grid were still connected. Make a new grid and
				// add it to the manager.
				List<Coords> surroundingTiles = placeable.GetSurroundingTiles();
				for(int i = 0; i < surroundingTiles.Count; i++)
				{
					// Find all reachable items from this coord
					List<Placeable> reachables = new List<Placeable>();
					List<Placeable> visited = new List<Placeable>();
					Queue<Placeable> q = new Queue<Placeable>();
					q.Enqueue(oldGrid.GetElemAt(surroundingTiles[i]));
					while(q.Count != 0)
					{
						Placeable elem = q.Dequeue();
						if(elem != null)
						{
							reachables.Add(elem);
							
							List<Coords> bounds = elem.GetBounds();
							visited.Add(elem);
							foreach(Coords coord in bounds)
							{
								// Check all the tiles surrounding this coord for
								// a non-visited element that isn't already in the q.
								List<Placeable> candidates = new List<Placeable>();
								candidates.Add(oldGrid.GetElemAt(coord + Vector2Int.up));
								candidates.Add(oldGrid.GetElemAt(coord + Vector2Int.right));
								candidates.Add(oldGrid.GetElemAt(coord + Vector2Int.left));
								candidates.Add(oldGrid.GetElemAt(coord + Vector2Int.down));
								foreach(Placeable candidate in candidates)
									if(candidate != null && 
										!visited.Contains(candidate) &&
										!q.Contains(candidate)
									)
									{
										q.Enqueue(candidate);
									}
							}
						}
					} // end of while(q.Count != 0)
					
					// If one of the surrounding tiles was 
					// reachable from another, remove it from
					// our list so we don't double count
					for(int j = i + 1; j < surroundingTiles.Count; j++)
						foreach(Placeable elem in reachables)
							if(elem.GetBounds().Contains(surroundingTiles[j])) {
								surroundingTiles.RemoveAt(j--);
							}
							
					// Create a new PowerGrid based on
					// the reachable list
					if(reachables.Count > 0)
					{
						PowerGrid newGrid = new PowerGrid();
						foreach(Placeable elem in reachables)
						{
							newGrid.Add(elem);
						}
						grids.Add(newGrid);
											
						// Update the posToGrid map for all tiles
						foreach(Placeable elem in reachables)
							foreach(Coords coord in elem.GetBounds())
								posToGridMap[coord] = newGrid;
					}
				}
			} // end of PlaceableDestroyedEventHandler
			
			public bool IsPowered(Placeable placeable)
			{
				return posToGridMap[placeable.coords].IsPowered(placeable);
			}
			
		} // end of PowerGridManager
		
		public class PowerGrid
		{
			public List<Building> inputs;
			public List<Building> outputs;
			public List<Cable> cables;
			
			public PowerGrid()
			{
				inputs = new List<Building>();
				outputs = new List<Building>();
				cables = new List<Cable>();
				
				Debug.Log("Created power grid");
			}
			
			~PowerGrid()
			{
				Debug.Log("Deleted power grid");
			}
			
			public Placeable GetElemAt(Coords coord)
			{
				foreach(Building building in inputs)
					if(building.GetBounds().Contains(coord))
						return building;
				foreach(Building building in outputs)
					if(building.GetBounds().Contains(coord))
						return building;	
				foreach(Cable cable in cables)
					if(cable.GetBounds().Contains(coord))
						return cable;	
					
				return null;
			}
			
			public void Add(Placeable placeable)
			{
				if(placeable.IsBuilding())
				{
					Building building = placeable.GetComponent<Building>();
					if(building.generatedResourceType == ResourceType.Power)
						inputs.Add(building);
					if(building.powerCost > 0)
						outputs.Add(building);
				}
				if(placeable.IsCable())
					cables.Add(placeable.GetComponent<Cable>());
			}
			
			public bool IsPowered(Placeable placeable)
			{
				/*float generated = 0;
				foreach(Building building in inputs)
					generated += building.GetGeneratedResourceCount();*/
				
				if(placeable.IsBuilding())
				{
					Building building = placeable.GetComponent<Building>();
					if(inputs.Contains(building))
						return true;
					if(outputs.Contains(building)) {
						if(inputs.Count > 0)
							return true;
						return false;
					}
				}

				return false;
			}
		}

	}
}