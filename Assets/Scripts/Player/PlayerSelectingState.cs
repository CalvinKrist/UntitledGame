using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
	namespace Controller
	{
		public class PlayerSelectingState : PlayerState
		{
			
			private bool active = false;
			
			public override void OnStateInitialize (Player player) 
			{
				base.OnStateInitialize(player);
				
				// Exit state if a placeable object button is selected
				ObjectSelector.OnPlaceableButtonSelectedEvent += (Placeable obj) => {
					if(active)
					{
						player.SwitchState<PlayerPlacingState>();
						player.GetState<PlayerPlacingState>().SetPlaceable(obj);
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
			}
		}
	}
}
