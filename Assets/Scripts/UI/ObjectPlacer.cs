using Untitled.Resource;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;
using UnityEngine.Tilemaps;
using Untitled;
using Untitled.Utils;

public class ObjectPlacer : MonoBehaviour
{
	// Prefab -- template of what we're making
    private Placeable toSpawn;
	// Actual object being placed
	public GameObject nextSpawn;
	[SerializeField] private ResourceStorage wallet;

	public string TileMapName = "Tilemap";

	private TileManager tileManager;
	
	public bool placed;

	public void Start()
	{
		tileManager = GameObject.Find(TileMapName).GetComponent<TileManager>();
	}

	private void Spawn(Coords coords)
	{
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance >= toSpawn.cost)
		{
			nextSpawn.GetComponent<Placeable>().Move(coords);
			placed=true;
			OnPlaceablePlacedEvent?.Invoke(nextSpawn.GetComponent<Placeable>());
			
			// Player pays the cost
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			
			// Configure building if it exists
			Building building = nextSpawn.GetComponent<Building>();
			if(building != null)
			{
				building.destinationStorage = wallet;
				building.inputStorage = GridUtils.GetResourceTileAt(coords);
			}
		}
	}

	public void Update()
	{
		if(Player.Instance.state == PlayerState.Placing)
		{
			Coords coords =  GridUtils.WorldToCoords(Input.mousePosition);
			nextSpawn.transform.position = coords.AsTile();
			
			if (Input.GetMouseButtonDown(0))
			{
				//generate list of coords from desired placing

				var tileType = GridUtils.GetTileTypeAt(coords);
				if (tileManager && toSpawn.placeableTiles.Contains(tileType)) 
				{
					Spawn(coords);
					
					Player.Instance.OnStateChange(PlayerState.Selecting);
				}
			
			}
		}
	}
	
	public void SetObject(Placeable newObject) 
	{
		toSpawn = newObject;
		nextSpawn = Instantiate(toSpawn.gameObject);
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Placeable> OnPlaceablePlacedEvent;
}
