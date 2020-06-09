using Untitled.Resource;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Tiles;
using UnityEngine.Tilemaps;

public class BuildingBuyer : MonoBehaviour
{
    public Building toSpawn;
	[SerializeField] private ResourceStorage wallet;

	public string TileMapName = "Tilemap";

	private TileManager tileManager;

	private bool _placing = false;

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
			created.GetComponent<Building>().destinationStorage = wallet;
			created.GetComponent<Building>().inputStorage = tileManager.GetResourceTileAtWorldCoords(position);
		}
	}

	public void StartPlacing()
	{
		_placing = true;
	}

	public void Update()
	{
		if(_placing && Input.GetMouseButtonDown(0))
		{
			var mousePos = Input.mousePosition;

			var tileType = tileManager.CheckType(mousePos);
			if (tileManager && tileType == TileType.Coal && toSpawn.placeableTiles.Contains(tileType))
				Spawn(mousePos);
			_placing = false;
		}
	}
}
