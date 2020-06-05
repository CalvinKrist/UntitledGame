using System.Collections;
using UnityEngine;
using System.Security.AccessControl;
using System.Collections.Generic;
using System;

namespace Untitled
{
    namespace Resource
    {
        public class ResourceStorage : MonoBehaviour
        {

            private Dictionary<ResourceType, float> resources;

            void Awake()
            {
                resources = new Dictionary<ResourceType, float>();
            }

            public void AddResources(ResourceType type, float count)
            {
                if (resources.ContainsKey(type))
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

    }
}
