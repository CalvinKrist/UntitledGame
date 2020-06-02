using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnClick : MonoBehaviour
{
    public GameObject toSpawn;

	private void Spawn(Vector3 position)
	{
		Instantiate(toSpawn).transform.position = position;
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
