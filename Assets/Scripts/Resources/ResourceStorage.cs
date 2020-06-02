using System.Collections;
using UnityEngine;
using ResourceTypes;
using System.Security.AccessControl;
using System.Collections.Generic;

public class ResourceStorage : MonoBehaviour
{

    private Dictionary<ResourceType, int> resources;

    public int STARTING_MONEY = 0;

    void Start()
    {
        resources = new Dictionary<ResourceType, int>();

        resources[ResourceType.Money] = STARTING_MONEY;
    }

    public void AddResources(ResourceType type, int count)
    {
        if(resources.ContainsKey(type))
            resources[type] += count;
        else
            resources[type] = count;
    }

    public int GetResourceCount(ResourceType type)
    {
        return resources[type];
    }

}
