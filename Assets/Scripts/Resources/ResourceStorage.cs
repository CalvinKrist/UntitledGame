using System.Collections;
using UnityEngine;
using ResourceTypes;
using System.Security.AccessControl;
using System.Collections.Generic;

public class ResourceStorage : MonoBehaviour
{

    private Dictionary<ResourceType, int> resources;

    void Start()
    {
        resources = new Dictionary<ResourceType, int>();
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
        if (resources.ContainsKey(type))
            return resources[type];
        else
            return 0;
    }

}
