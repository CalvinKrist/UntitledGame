using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Utils;
using Untitled.Resource;

namespace Untitled
{
	namespace Controller
	{
		public class PlayerPlacingState : PlayerState
		{
			private Placeable toSpawn;
			private ResourceStorage wallet;
			
			private bool active = false;
			
			public override void OnStateInitialize(Player player) 
			{
				base.OnStateInitialize(player);
				wallet = player.GetComponent<ResourceStorage>();
				
				ObjectSelector.OnPlaceableButtonSelectedEvent += (Placeable obj) => {
					if(active)
					{
						SetPlaceable(obj);
					}
				};
			}
			
			public override void OnStateEnter()
			{
				active = true;
			}
			public override void OnStateExit()
			{
				active = false;
			}
			
			public override void Update()
			{
				// Exit state is 'Escape' is pressed or
				// if right click is pressed
				if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
				{
					player.SwitchState<PlayerSelectingState>();
				}
				
				// If left mouse is clicked attempt to place object
				if(Input.GetMouseButtonDown(0))
				{
					Coords coords =  GridUtils.WorldToCoords(Input.mousePosition);

					var tileType = GridUtils.GetTileTypeAt(coords);
					if (toSpawn.placeableTiles.Contains(tileType)) {
						Spawn(coords);
					}
				
				}
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
					var created = GameObject.Instantiate(toSpawn.gameObject, coords.AsTile(), Quaternion.Euler(0,0,0));
					
					// Configure building if it exists
					Building building = created.GetComponent<Building>();
					if(building != null)
					{
						building.destinationStorage = wallet;
						building.inputStorage = GridUtils.GetResourceTileAt(coords);
					}
				}
			}
			
			public void SetPlaceable(Placeable obj)
			{
				toSpawn = obj;
			}
		}
	}
}
