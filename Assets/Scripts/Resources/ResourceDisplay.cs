using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.UI;
using ResourceTypes;

public class ResourceDisplay : MonoBehaviour
{

    // Must be have ResourceStorage component
    public ResourceStorage storage;
    public ResourceType type;

    private Text textComponent;

    void Start()
    {
        textComponent = this.gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(textComponent)
            textComponent.text = storage.GetResourceCount(type).ToString();
    }
}
