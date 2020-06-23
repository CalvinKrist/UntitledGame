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
				
				ClickableSprite.OnSpriteClickEvent += (ClickableSprite sprite) => {
					if(active)
					{
						GameObject.Destroy(sprite.gameObject);
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
