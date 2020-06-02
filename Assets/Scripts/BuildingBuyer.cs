using ResourceTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingBuyer : MonoBehaviour
{
    public Building toSpawn;
	[SerializeField] private ResourceStorage wallet;
	private void Spawn(Vector3 position)
	{
		int balance = wallet.GetResourceCount(ResourceType.Money);
		if(balance < toSpawn.cost)
			Debug.Log("not enough money to place");
		else
		{
			wallet.AddResources(ResourceType.Money, -toSpawn.cost);
			Instantiate(toSpawn).transform.position = position;
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
			Vector3? terrainRayPosition = ClickToGround();
			if(terrainRayPosition.HasValue)
			{
				this.Spawn((Vector3)terrainRayPosition);
			}
		}
	}
}
