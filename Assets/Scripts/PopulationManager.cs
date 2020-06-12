using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Untitled.Resource;

/*
* Helps manage assignments of pops between 
* buildings, keeps track of total / unassigned /
* assigned pops, pop generation rate, and money
* generation rate.
*
* Uses the Singleton pattern so other managers, UIs,
* and buildings can change their pops
*/
public class PopulationManager : MonoBehaviour
{
	
	#region SINGLETON PATTERN
	public static PopulationManager _instance;
	public static PopulationManager Instance
	{
	 get {
		 if (_instance == null)
		 {
			 _instance = GameObject.FindObjectOfType<PopulationManager>();
			 
			 if (_instance == null)
			 {
				 GameObject container = new GameObject("GameController");
				 _instance = container.AddComponent<PopulationManager>();
			 }
		 }
	 
		 return _instance;
	 }
	}
	#endregion
	
	private int totalPopulation;
	private int assignedPopulation;
	
	public ResourceStorage playerStorage;
	
	public void Start() {
		totalPopulation = (int)playerStorage.GetResourceCount(ResourceType.Population);
		assignedPopulation = 0;
	}
	
	public static bool AssignWorker(Building destination) {
		PopulationManager pop = PopulationManager.Instance;
		ResourceStorage destStorage = destination.GetComponent<ResourceStorage>();
		
		if(pop.playerStorage.GetResourceCount(ResourceType.Population) < 1 || destStorage.GetResourceCount(ResourceType.Population) > destination.maxPops - 1)
			return false;
		
		pop.playerStorage.AddResources(ResourceType.Population, -1);
		destStorage.AddResources(ResourceType.Population, 1);
		
		return true;
	}
	
	public static bool UnassignWorker(Building destination) {
		PopulationManager pop = PopulationManager.Instance;
		ResourceStorage destStorage = destination.GetComponent<ResourceStorage>();
		
		if(destStorage.GetResourceCount(ResourceType.Population) < 1)
			return false;
		
		pop.playerStorage.AddResources(ResourceType.Population, 1);
		destStorage.AddResources(ResourceType.Population, -1);
		
		return true;
	}
}
