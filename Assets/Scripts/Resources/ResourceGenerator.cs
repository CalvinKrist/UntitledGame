using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceTypes;

// TODO: support muyltiple storages on same object
public class ResourceGenerator : MonoBehaviour
{
    public ResourceType type;
    public int income;

    public GameObject destination;

    void Update()
    {
        destination.GetComponent<ResourceStorage>().AddResources(type, income);
    }
}
