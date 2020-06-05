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
	private void Spawn(Vector3 position)
	{
		float balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance < toSpawn.cost)
			Debug.Log("not enough money to place");
		else
		{
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			var created = Instantiate(toSpawn);
			created.transform.position = position;
			created.GetComponent<ResourceGenerator>().destination = wallet;
		}
	}

	private Vector3? ClickToGround()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.NameToLayer("Terrain")))
		{
			return hit.point;
		}
		return null;
	}

	public void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log("Mouse Position: " + pos);
			TileManager t = GetComponent<TileManager>();
			Debug.Log("Look at meeeeeeee TileType: " + t.CheckType(pos));
			pos.z = 0;
			this.Spawn(pos);
		}
	}
}
