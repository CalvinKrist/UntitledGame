using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Untitled.Resource;
using Untitled.Tiles;
using Untitled.Configs;
using UnityEngine.UI;
using Untitled.UI;
using System;
using Untitled.Utils;

[RequireComponent(typeof(ResourceStorage))]
[RequireComponent(typeof(SpriteRenderer))]
public class Building : AClickableSprite
{
	[Header("General Settings")]
	public string name;
    // Cost to place
	[field: SerializeField]
	public int cost {get; set;}
	public float moneyGen = 20; // measured per minute with full pop
	private float moneyGenerationRate; // generation per second
    // How much electricity it needs to function
    public float powerCost;
	// Max number of pops that can work the building
	public int maxPops;
	private float invMaxPops; // precomputed for faster math
	// What tiles the building can be placed on
	[field: SerializeField]
	public List<TileType> placeableTiles {get; set;}
	[field: SerializeField]
	public Vector2 size {get; set;}

    // Resource output variables
    [Header("Resource Output")]
    public ResourceType generatedResourceType = ResourceType.None;
    public float resourceGenerationRate; // rate per minute
    private float generationPerSec;
    public IResourceStorage destinationStorage;

    // Resource input variables
    [Header("Resource Input")]
    public ResourceType resourceInputType = ResourceType.None;
    public float resourceDepletionRate; // rate per minute
    private float depletionPerSec;
    public IResourceStorage inputStorage;
	
	private ResourceStorage storage;

    void Awake()
    {
        // Set resource rates per second
        if (generatedResourceType != ResourceType.None)
            generationPerSec = resourceGenerationRate / 60f;
        if (resourceInputType != ResourceType.None)
            depletionPerSec = resourceDepletionRate / 60f;
		
		this.spriteType = SpriteType.Building;
		
		// Add resource storage to parent if it doesn't exist
		if(GetComponent<ResourceStorage>() == null)
		{
			this.gameObject.AddComponent<ResourceStorage>();
		}
		
		moneyGenerationRate = moneyGen / 60;
		
		invMaxPops = 1f / maxPops;
		
		this.storage = GetComponent<ResourceStorage>();
    }
	
	void Start()
	{
		base.Start();
		
		// Invoke building created event
		OnBuildingCreateEvent?.Invoke(this);
	}
	
	void OnDestroy() 
	{
		base.OnDestroy();
		
		while(storage.GetResourceCount(ResourceType.Population) > 0)
			PopulationManager.UnassignWorker(this);
		
		OnBuildingDestroyEvent?.Invoke(this);
	}

	private float GetPopScalar()
	{
		return storage.GetResourceCount(ResourceType.Population) * invMaxPops;
	}
	
    void Update()
    {	
        if(destinationStorage != null && generatedResourceType != ResourceType.None && 
            (resourceInputType == ResourceType.None || 
            (inputStorage != null && inputStorage.GetResourceCount(resourceInputType) >= depletionPerSec * Time.deltaTime)))
        {
			// TODO: update to include fractional workers, or full if max pops = 0
            destinationStorage.AddResources(generatedResourceType, generationPerSec * Time.deltaTime * GetPopScalar());

            if(!(resourceInputType == ResourceType.None))
            {
                inputStorage.AddResources(resourceInputType, -depletionPerSec * Time.deltaTime * GetPopScalar());
            }
        } 
    }
	
	// Returns how much money per second the building generates
	public float GetMoneyIncome() {
		return moneyGenerationRate * GetPopScalar();;
	}
	
	// Returns how much resource per second the building generates
	public float GetResourceIncome() {
		return generationPerSec * GetPopScalar();;
	}
	
	// Returns how much power the building
	// requires from the grid to function
	public float GetPowerDrain()
	{
		return powerCost;
	}
	
	/***************
	***  Events  ***
	****************/
	public static event Action<Building> OnBuildingCreateEvent;
	public static event Action<Building> OnBuildingDestroyEvent;
	
}
