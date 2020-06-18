using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Untitled.Tiles;
using System;
using Untitled.Utils;

public class Placeable : MonoBehaviour
{
    public int cost;
	public List<TileType> placeableTiles;
	public Vector2Int size;
	public Coords coords;
	private List<Coords> boundsList;
	
	protected void Start()
	{
		coords = new Coords(this.gameObject.transform.position);
		
		boundsList = new List<Coords>();
		
		// Calculate offset bounds rounding down 
		int xStart = -(int)((size.x - 1) / 2);
		int xEnd = (int)(size.x / 2);
		int yStart = -(int)((size.y - 1) / 2);
		int yEnd = (int)(size.y / 2);

		for(int xOff = xStart; xOff <= xEnd; xOff++)
			for(int yOff = yStart; yOff <= yEnd; yOff++)
				boundsList.Add(coords + new Vector2Int(xOff, -yOff));
		
		OnPlaceableCreateEvent?.Invoke(this);
	}
	
	protected void OnDestroy() 
	{
		OnPlaceableDestroyEvent?.Invoke(this);
	}
	
	public bool IsBuilding()
	{
		return this.gameObject.GetComponent<Building>() != null;
	}
	public bool IsCable()
	{
		return this.gameObject.GetComponent<Cable>() != null;
	}
	public bool IsNextTo(Placeable other)
	{
		// Create a list of surrounding coords 
		List<Coords> placeableTiles = this.GetBounds();
		List<Coords> surroundingTiles = new List<Coords>();
		foreach(Coords coords in placeableTiles)
		{
			if(!placeableTiles.Contains(coords + Vector2Int.up))
				surroundingTiles.Add(coords + Vector2Int.up);
			if(!placeableTiles.Contains(coords + Vector2Int.right))
				surroundingTiles.Add(coords + Vector2Int.right);
			if(!placeableTiles.Contains(coords + Vector2Int.down))
				surroundingTiles.Add(coords + Vector2Int.down);
			if(!placeableTiles.Contains(coords + Vector2Int.left))
				surroundingTiles.Add(coords + Vector2Int.left);
		}
		
		foreach(Coords coord in surroundingTiles)
		{
			if(GridUtils.GetPlaceableAt(coord) == other)
				return true;
		}
		
		return false;
	}
	
	// Returns a list of Coord objects for all
	// tiles the placeable is on
	public List<Coords> GetBounds()
	{
		return boundsList;
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Placeable> OnPlaceableCreateEvent;
	public static event Action<Placeable> OnPlaceableDestroyEvent;
}
