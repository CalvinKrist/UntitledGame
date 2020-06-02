using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceTypes;

public class ResourceGenerator : MonoBehaviour
{
    public ResourceType type;
    public int income;

    public GameObject destination;
    private ResourceStorage dest;

    void Start()
    {
        dest = destination.GetComponent<ResourceStorage>();
    }

    void Update()
    {
        if(dest)
            dest.AddResources(type, income);
    }
}
