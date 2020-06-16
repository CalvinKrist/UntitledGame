using Untitled.Resource;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;
using UnityEngine.Tilemaps;
using Untitled;

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

	private void Spawn(Vector3 position)
	{
		Debug.Log("PLACING AND SPAWNING");
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance >= toSpawn.cost)
		{
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			var created = Instantiate(toSpawn.gameObject);
			created.transform.position = tileManager.CastWorldCoordsToTile(position);
			
			// Configure building if it exists
			Building building = created.GetComponent<Building>();
			if(building != null)
			{
				building.destinationStorage = wallet;
				building.inputStorage = tileManager.GetResourceTileAtWorldCoords(position);
			}
		}
	}

	public void Update()
	{
		if(Player.Instance.state == PlayerState.Placing && Input.GetMouseButtonDown(0))
		{
			var mousePos = Input.mousePosition;

			var tileType = tileManager.CheckType(mousePos);
			if (tileManager && toSpawn.placeableTiles.Contains(tileType)) {
				Spawn(mousePos);
				Player.Instance.OnStateChange(PlayerState.Selecting);
			}
		
		}
	}
	
	public void SetObject(Placeable newObject) 
	{
		toSpawn = newObject;
	}
}
