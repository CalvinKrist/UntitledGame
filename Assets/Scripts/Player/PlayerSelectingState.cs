using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
	namespace Controller
	{
		public class PlayerSelectingState : PlayerState
		{
			public override void OnStateInitialize (Player player) 
			{
				base.OnStateInitialize(player);
				
				// Exit state if a placeable object button is selected
				ObjectSelector.OnPlaceableButtonSelectedEvent += (Placeable obj) => {
					player.SwitchState<PlayerPlacingState>();
					player.GetState<PlayerPlacingState>().SetPlaceable(obj);
				};
			}
			
			public override void OnStateEnter()
			{
				
			}
			public override void OnStateExit()
			{
				GameObject.Find("BuildingBuyerSelection").BroadcastMessage("Deselect");
			}
			public override void Update()
			{
			}
		}
	}
}
