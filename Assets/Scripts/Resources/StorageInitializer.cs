using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ResourceTypes;

public class StorageInitializer : MonoBehaviour
{

    public int STARTING_POWER = 0;
    public int STARTING_MONEY = 0;
	public int STARTING_POPULATION = 0;

   public void Initialize(ResourceStorage storage)
    {
        storage.AddResources(ResourceType.Money, STARTING_MONEY);
        storage.AddResources(ResourceType.Power, STARTING_POWER);
		storage.AddResources(ResourceType.Population, STARTING_POPULATION);
    }
}
