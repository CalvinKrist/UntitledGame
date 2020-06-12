﻿using Untitled.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;
using UnityEngine.Tilemaps;
using Untitled;

public class BuildingBuyer : MonoBehaviour
{
    private Building toSpawn;
	[SerializeField] private ResourceStorage wallet;

	public string TileMapName = "Tilemap";

	private TileManager tileManager;

	public void Start()
	{
		tileManager = GameObject.Find(TileMapName).GetComponent<TileManager>();
	}

	private void Spawn(Vector3 position)
	{
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance >= toSpawn.cost)
		{
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			var created = Instantiate(toSpawn);
			created.transform.position = tileManager.CastWorldCoordsToTile(position);
			Building building = created.GetComponent<Building>();
			building.destinationStorage = wallet;
			building.inputStorage = tileManager.GetResourceTileAtWorldCoords(position);
		}
	}

	public void Update()
	{
		if(Player.Instance.state == PlayerState.Placing && Input.GetMouseButtonDown(0))
		{
			var mousePos = Input.mousePosition;

			var tileType = tileManager.CheckType(mousePos);
			if (tileManager && tileType == TileType.Coal && toSpawn.placeableTiles.Contains(tileType)) {
				Spawn(mousePos);
				Player.Instance.OnStateChange(PlayerState.Selecting);
			}
		
		}
	}
	
	public void SetBuilding(Building newBuilding) 
	{
		toSpawn = newBuilding;
	}
}
