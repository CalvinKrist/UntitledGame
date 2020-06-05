using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Untitled
{
    namespace Resource
    {
        public class ResourceGenerator : MonoBehaviour
        {
            public ResourceType type;
            public float income; // measured as income per minute
            public ResourceStorage destination;

            void FixedUpdate()
            {
                destination.AddResources(type, income * Time.deltaTime / 60);
            }
        }
    }
}