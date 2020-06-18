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
    private Placeable toSpawn;
	[SerializeField] private ResourceStorage wallet;

	public string TileMapName = "Tilemap";

	private TileManager tileManager;

	public void Start()
	{
		tileManager = GameObject.Find(TileMapName).GetComponent<TileManager>();
	}

	private void Spawn(Coords coords)
	{
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance >= toSpawn.cost)
		{
			// Player pays the cost
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			
			// Create the placeable at the specified position
			// so that it's position is already set when PlaceableCreated
			// events are generated
			var created = Instantiate(toSpawn.gameObject, coords.AsTile(), Quaternion.Euler(0,0,0));
			
			// Configure building if it exists
			Building building = created.GetComponent<Building>();
			if(building != null)
			{
				building.destinationStorage = wallet;
				building.inputStorage = GridUtils.GetResourceTileAt(coords);
			}
		}
	}

	public void Update()
	{
		if(Player.Instance.state == PlayerState.Placing && Input.GetMouseButtonDown(0))
		{
			Coords coords =  GridUtils.WorldToCoords(Input.mousePosition);

			var tileType = GridUtils.GetTileTypeAt(coords);
			if (tileManager && toSpawn.placeableTiles.Contains(tileType)) {
				Spawn(coords);
				Player.Instance.OnStateChange(PlayerState.Selecting);
			}
		
		}
	}
	
	public void SetObject(Placeable newObject) 
	{
		toSpawn = newObject;
	}
}
