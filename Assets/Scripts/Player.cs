using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Resource;
using UnityEngine.UI;
using System;

namespace Untitled
{
	public enum PlayerState {
		Placing,
		Selecting
	};
	
	[RequireComponent(typeof(ResourceStorage))]
	[RequireComponent(typeof(PopulationManager))]
	[RequireComponent(typeof(BuildingBuyer))]
	public class Player : MonoBehaviour
	{
		#region SINGLETON PATTERN
		public static Player _instance;
		public static Player Instance
		{
		 get {
			 if (_instance == null)
			 {
				 _instance = GameObject.FindObjectOfType<Player>();
				 
				 if (_instance == null)
				 {
					 GameObject container = new GameObject("GameController");
					 _instance = container.AddComponent<Player>();
				 }
			 }
		 
			 return _instance;
		 }
		}
		#endregion
		
		public PlayerState state;
		
		// Start is called before the first frame update
		void Start()
		{
			state = PlayerState.Selecting;
		}
			
		void Update()
		{	
			// Use 'escape' to exit building placing mode
			if(state == PlayerState.Placing && Input.GetKeyDown(KeyCode.Escape) == true)
			{
				OnStateChange(PlayerState.Selecting);
			}
		}
			
		/*************************
		****  Event Handlers  ****
		**************************/
		public void OnStateChange(PlayerState newState) 
		{
			if(this.state == PlayerState.Placing && newState == PlayerState.Selecting)
				GameObject.Find("BuildingBuyerSelection").BroadcastMessage("Deselect");
			this.state = newState;
		}

	}

}