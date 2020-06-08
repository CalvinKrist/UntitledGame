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

	public void Start()
	{
		tileManager = GameObject.Find(TileMapName).GetComponent<TileManager>();
	}

	private void Spawn(Vector3 position)
	{
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance < toSpawn.cost)
			Debug.Log("not enough money to place");
		else
		{
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			var created = Instantiate(toSpawn);
			created.transform.position = tileManager.CastWorldCoordsToTile(position);
			created.GetComponent<Building>().destinationStorage = wallet;
			created.GetComponent<Building>().inputStorage = tileManager.GetResourceTileAtWorldCoords(position);
			
			Debug.Log(created.GetComponent<Building>().inputStorage);
		}
	}

	public void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			var mousePos = Input.mousePosition;
			
			Debug.Log(tileManager.CheckType(mousePos));

			var tileType = tileManager.CheckType(mousePos);
			if (tileManager && tileType == TileType.Coal && toSpawn.placeableTiles.Contains(tileType))
				Spawn(mousePos);
		}
	}
}
