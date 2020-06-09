using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
    namespace Resource
    {
        
        public class StorageInitializer : MonoBehaviour
        {

            public int STARTING_POWER = 0;
            public int STARTING_MONEY = 0;
			public int STARTING_COAL = 0;

            public void Initialize(IResourceStorage storage)
            {
                storage.AddResources(ResourceType.Money, STARTING_MONEY);
                storage.AddResources(ResourceType.Power, STARTING_POWER);
				storage.AddResources(ResourceType.Coal, STARTING_COAL);
            }
        }

    }
}
