using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.UI;
using ResourceTypes;

public class ResourceDisplay : MonoBehaviour
{

    // Must be have ResourceStorage component
    public GameObject storage;
    public ResourceType type;

    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = storage.GetComponent<ResourceStorage>().GetResourceCount(type).ToString();
    }
}
