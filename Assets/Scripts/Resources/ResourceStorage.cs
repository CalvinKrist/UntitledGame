using System.Collections;
using UnityEngine;
using ResourceTypes;
using System.Security.AccessControl;
using System.Collections.Generic;

public class ResourceStorage : MonoBehaviour
{

    private Dictionary<ResourceType, float> resources;

    public StorageInitializer initializer = null;

    void Awake()
    {
        resources = new Dictionary<ResourceType, float>();

        if(initializer != null)
        {
            initializer.Initialize(this);
        }
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
        if (resources.ContainsKey(type))
            return resources[type];
        return 0;
    }

}
