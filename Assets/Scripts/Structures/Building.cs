using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Untitled.Resource;
using Untitled.Tiles;

public class Building : MonoBehaviour
{
    // Cost to place
	public int cost;
    // How much electricity it needs to function
    public int powerCost;
	// What tiles the building can be placed on
	public List<TileType> placeableTiles;

    // Resource output variables
    [Header("Resource Output")]
    public ResourceType generatedResourceType = ResourceType.None;
    public int resourceGenerationRate; // rate per minute
    private float generationPerSec;
    public IResourceStorage destinationStorage;

    // Resource input variables
    [Header("Resource Input")]
    public ResourceType resourceInputType = ResourceType.None;
    public int resourceDepletionRate; // rate per minute
    private float depletionPerSec;
    public IResourceStorage inputStorage;

    void Start()
    {
        // Set resource rates per second
        if (generatedResourceType != ResourceType.None)
            generationPerSec = resourceGenerationRate / 60f;
        if (resourceInputType != ResourceType.None)
            depletionPerSec = resourceDepletionRate / 60f;
    }

    void Update()
    {	
	
        if(destinationStorage != null && generatedResourceType != ResourceType.None && 
            (resourceInputType == ResourceType.None || 
            (inputStorage != null && inputStorage.GetResourceCount(resourceInputType) >= depletionPerSec * Time.deltaTime)))
        {
            destinationStorage.AddResources(generatedResourceType, generationPerSec * Time.deltaTime);

            if(!(resourceInputType == ResourceType.None))
            {
                inputStorage.AddResources(resourceInputType, -depletionPerSec * Time.deltaTime);
            }
        } 
    }
}
