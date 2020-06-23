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
	namespace Controller
	{
		[RequireComponent(typeof(GridUtils))]
		[RequireComponent(typeof(ResourceStorage))]
		[RequireComponent(typeof(PopulationManager))]
		// Functions as an OO state machine with PlayerStates
		// Each state is responsible for subscribing to and
		// publishing their own events to result in state transitions.
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
			
			/************************************
			****   State machine variables   ****
			*************************************/
			private List<PlayerState> statesList;
			private PlayerState currentState;
			
			/************************************
			****   State machine functions   ****
			*************************************/
			private bool SwitchState (PlayerState state) 
			{
				bool success = false;
				if (state && state != currentState) {
					if (currentState)
						currentState.OnStateExit();
					currentState = state;
					currentState.OnStateEnter();
					success = true;
				}
				return success;
			}
			
			public bool SwitchState<StateType> () where StateType : PlayerState, new() 
			{
				bool success = false;
				bool found = false;
				//if the state can be found in the list of states 
				//already created, switch to the existing version
				foreach (PlayerState state in statesList) {
					if (state is StateType) {
						found = true;
						success = SwitchState(state);
						break;
					}
				}
				//if the state is not found in the list, 
				//make a new instance
				if (!found) {
					PlayerState newState = new StateType();
					newState.OnStateInitialize(this);
					statesList.Add(newState);
					success = SwitchState(newState);
				}
				return success;
			}
			
			public StateType GetState<StateType> () where StateType : PlayerState, new() 
			{
				foreach (PlayerState state in statesList) 
					if (state is StateType)
						return (StateType)state;
				

				// State not found: create it
				PlayerState newState = new StateType();
				newState.OnStateInitialize(this);
				statesList.Add(newState);
				return (StateType)newState;
			}
			
			public PlayerState GetCurrentState()
			{
				return currentState;
			}
			
			/*********************************
			****   Player Functionality   ****
			**********************************/
			
			void Awake()
			{							
				storage = GetComponent<ResourceStorage>();
				buildings = new List<Building>();
				Placeable.OnPlaceableCreateEvent += (Placeable placeable) => {
					if(placeable.IsBuilding())
						buildings.Add(placeable.gameObject.GetComponent<Building>() );
				};
				Placeable.OnPlaceableDestroyEvent += (Placeable placeable) => {
					if(placeable.IsBuilding())
						buildings.Remove(placeable.gameObject.GetComponent<Building>());
				};
			
				statesList = new List<PlayerState> ();
				
				// Access the PowerGrid manager to instantiate it
				var gridManager = PowerGridManager.Instance;
				// Access the GridUtils manager to instantiate it
				var gridUtils = GridUtils.Instance;
			}
			
			void Start()
			{
				SwitchState<PlayerSelectingState>();
			}
				
			void Update()
			{	
				currentState.Update();
				
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
		}
	}

}