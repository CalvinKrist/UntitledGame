using System.Collections;
using UnityEngine;
using ResourceTypes;
using System.Security.AccessControl;
using System.Collections.Generic;

public class ResourceStorage : MonoBehaviour
{

    private Dictionary<ResourceType, float> resources;

    public int STARTING_POWER = 0;
    public int STARTING_MONEY = 0;

    void Start()
    {
        resources = new Dictionary<ResourceType, float>();

        resources[ResourceType.Power] = STARTING_POWER;
        resources[ResourceType.Money] = STARTING_MONEY;
    }

    public void AddResources(ResourceType type, float count)
    {
        if(resources.ContainsKey(type))
            resources[type] += count;
        else
            resources[type] = count;
    }

    public float GetResourceCount(ResourceType type)
    {
        return resources[type];
    }

}
