using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.UI;

namespace Untitled
{
	namespace Controller
	{
		public class PlayerDeletingState : PlayerState
		{
			private bool active = false;
			
			public override void OnStateInitialize (Player player) 
			{
				base.OnStateInitialize(player);
				
				// Event subscription for when a sprite is clicked
				ClickableSprite.OnSpriteClickEvent += (ClickableSprite sprite) => {
					if(active)
					{
						GameObject.Destroy(sprite.gameObject);
					}
				};
				
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
				UI_Manager.Instance.ToggleOnDeleteButton();
			}
			public override void OnStateExit()
			{
				active = false;
				UI_Manager.Instance.ToggleOffDeleteButton();
			}
			public override void Update()
			{
				if(Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
				{
					player.SwitchState<PlayerSelectingState>();
				}
			}
		}
	}
}
