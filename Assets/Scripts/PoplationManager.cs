using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceTypes;

/*
* Helps manage assignments of pops between 
* buildings, keeps track of total / unassigned /
* assigned pops, pop generation rate, and money
* generation rate.
*
* Uses the Singleton pattern so other managers, UIs,
* and buildings can change their pops
*/
public class PoplationManager : MonoBehaviour
{
	
	#region SINGLETON PATTERN
	public static PoplationManager _instance;
	public static PoplationManager Instance
	{
	 get {
		 if (_instance == null)
		 {
			 _instance = GameObject.FindObjectOfType<PoplationManager>();
			 
			 if (_instance == null)
			 {
				 GameObject container = new GameObject("GameController");
				 _instance = container.AddComponent<PoplationManager>();
			 }
		 }
	 
		 return _instance;
	 }
	}
	#endregion
	
	private int totalPopulation;
	private int assignedPopulation;
	
	public ResourceStorage playerStorage;
	
	public float moneyPerPop = 20; // measured per minute
	private float moneyGenerationRate; // generation per second
	
	public void Start() {
		totalPopulation = (int)playerStorage.GetResourceCount(ResourceType.Population);
		assignedPopulation = 0;
		moneyGenerationRate = moneyPerPop / 60;
	}
	
	public bool AssignWorker(Building destination) {
		return false;
	}
	
	public bool UnassignWorker(Building destination) {
		return false;
	}
	
	public void Update() {
		playerStorage.AddResources(ResourceType.Money, 
			moneyGenerationRate * Time.deltaTime * assignedPopulation);
	}
}
