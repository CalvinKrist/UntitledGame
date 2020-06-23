using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
	namespace Controller
	{
		public abstract class PlayerState {
			
			protected Player player;

			public PlayerState () 
			{

			}

			public static implicit operator bool (PlayerState state) 
			{
				return state != null;
			}

			public virtual void OnStateInitialize (Player player) 
			{
				this.player = player;
			}

			public abstract void OnStateEnter();
			public abstract void OnStateExit();
			public abstract void Update();
		}
	}
}
