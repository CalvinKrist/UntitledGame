using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Resource;
using UnityEngine.UI;
using System;
using Untitled.Power;
using Untitled.Utils;

namespace Untitled
{
	public enum PlayerState {
		Placing,
		Selecting
	};
	
	[RequireComponent(typeof(GridUtils))]
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
		
		
		private ResourceStorage storage;
		private List<Building> buildings;
		
		public PlayerState state;
		
		void Awake()
		{			
			state = PlayerState.Selecting;
			
			storage = GetComponent<ResourceStorage>();
			buildings = new List<Building>();
			
			Building.OnBuildingCreateEvent += (Building building) => {
				buildings.Add(building);
			};
			Building.OnBuildingDestroyEvent += (Building building) => {
				buildings.Remove(building);
			};
			
			// Access the PowerGrid manager to instantiate it
			var gridManager = PowerGridManager.Instance;
		}
			
		void Update()
		{	
			// Use 'escape' to exit building placing mode
			if(state == PlayerState.Placing && Input.GetKeyDown(KeyCode.Escape) == true)
			{
				OnStateChange(PlayerState.Selecting);
			}
			
			// Update income
			float incomePerSec = 0;
			foreach(Building building in buildings)
				incomePerSec += building.GetMoneyIncome();
			storage.AddResources(ResourceType.Money, incomePerSec * Time.deltaTime);
		}
		
		public ResourceStorage GetStorage()
		{
			return GetComponent<ResourceStorage>();
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