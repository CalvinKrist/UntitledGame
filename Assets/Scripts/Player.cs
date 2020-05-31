using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public int startingResource = 100;

    private int resource;

    // Start is called before the first frame update
    void Start()
    {
        resource = startingResource;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getResources()
    {
        return resource;
    }

    // TODO: make threadsafe?
    public void addResources(int inc)
    {
        resource += inc;
    }
}
