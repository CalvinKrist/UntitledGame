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
using Untitled.Power;

[RequireComponent(typeof(ResourceStorage))]
[RequireComponent(typeof(SpriteRenderer))]
public class Building : Placeable
{
	[Header("General Settings")]
	public string name;
    // Cost to place
	public float moneyGen = 20; // measured per minute with full pop
	private float moneyGenerationRate; // generation per second
    // How much electricity it needs to function
    public float powerCost;
	// Max number of pops that can work the building
	public int maxPops;
	private float invMaxPops; // precomputed for faster math

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
	
	[Header("Rendering Settings")]
	public Sprite poweredSprite;
	public Sprite unpoweredSprite;
	private SpriteRenderer renderer;
	
	private ResourceStorage storage;

    void Awake()
    {
		renderer = GetComponent<SpriteRenderer>();
		
        // Set resource rates per second
        if (generatedResourceType != ResourceType.None)
            generationPerSec = resourceGenerationRate / 60f;
        if (resourceInputType != ResourceType.None)
            depletionPerSec = resourceDepletionRate / 60f;
		
		// Add resource storage to parent if it doesn't exist
		if(GetComponent<ResourceStorage>() == null)
		{
			this.gameObject.AddComponent<ResourceStorage>();
		}
		
		moneyGenerationRate = moneyGen / 60;
		
		if(maxPops > 0)
			invMaxPops = 1f / maxPops;
		
		this.storage = GetComponent<ResourceStorage>();
    }
	
	void Start()
	{
		base.Start();
	}
	
	void OnDestroy() 
	{
		base.OnDestroy();
		
		while(storage.GetResourceCount(ResourceType.Population) > 0)
			PopulationManager.UnassignWorker(this);
	}

	private float GetPopScalar()
	{
		if(maxPops == 0)
			return 1;
		
		return storage.GetResourceCount(ResourceType.Population) * invMaxPops;
	}
	
	// Returns count of generated resources on this specific update
	// without factoring in if it's powered
	public float GetGeneratedResourceCount()
	{
		return generationPerSec * Time.deltaTime * GetPopScalar();
	}
	
    void Update()
    {	
		bool powered = PowerGridManager.Instance.IsPowered(GetComponent<Placeable>());
		int poweredFactor = 1;
		if(powered){
			renderer.sprite = poweredSprite;
		}
		else {
			renderer.sprite = unpoweredSprite;
			poweredFactor = 0;
		}
		
        if(destinationStorage != null && generatedResourceType != ResourceType.None && 
            (resourceInputType == ResourceType.None || 
            (inputStorage != null && inputStorage.GetResourceCount(resourceInputType) >= depletionPerSec * Time.deltaTime)))
        {
            destinationStorage.AddResources(generatedResourceType, GetGeneratedResourceCount() * poweredFactor);

            if(!(resourceInputType == ResourceType.None))
            {
                inputStorage.AddResources(resourceInputType, -depletionPerSec * Time.deltaTime * GetPopScalar() * poweredFactor);
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
	
}
