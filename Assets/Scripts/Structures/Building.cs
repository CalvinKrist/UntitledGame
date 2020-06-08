using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Untitled.Resource;

public class Building : MonoBehaviour
{
    // Cost to place
	public int cost;
    // How much electricity it needs to function
    public int powerCost;

    // Resource output variables
    [Header("Resource Output")]
    public ResourceType generatedResourceType = ResourceType.None;
    public int resourceGenerationRate; // rate per minute
    private float generationPerSec;
    public ResourceStorage destinationStorage;

    // Resource input variables
    [Header("Resource Input")]
    public ResourceType resourceInputType = ResourceType.None;
    public int resourceDepletionRate; // rate per minute
    private float depletionPerSec;
    public ResourceStorage inputStorage;

    void Start()
    {
        // Set resource rates per second
        if (generatedResourceType != ResourceType.None)
            generationPerSec = resourceGenerationRate / 60;
        if (resourceInputType != ResourceType.None)
            depletionPerSec = resourceDepletionRate / 60;
    }

    void Update()
    {
        if(destinationStorage && generatedResourceType != ResourceType.None && 
            (resourceInputType == ResourceType.None || 
            (inputStorage && inputStorage.GetResourceCount(resourceInputType) >= depletionPerSec * Time.deltaTime)))
        {
            destinationStorage.AddResources(generatedResourceType, generationPerSec * Time.deltaTime);

            if(!(resourceInputType == ResourceType.None))
            {
                inputStorage.AddResources(resourceInputType, -depletionPerSec * Time.deltaTime);
            }
        } 
    }
}
