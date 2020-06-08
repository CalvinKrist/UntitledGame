using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Untitled
{
    namespace Configs
    {
        public class Config : MonoBehaviour
        {
            private static Config instance;
            public static Config Instance { get { return instance; } }

            private void Awake()
            {
                if (instance == null)
                {
                    instance = this;
                    DontDestroyOnLoad(this.gameObject);
                } else
                {
                    Destroy(this);
                }
            }

            [SerializeField] private TileConfig tileConfig;
            [SerializeField] private ResourceConfig resourceConfig;

            public TileConfig TileConfig { get { return tileConfig; } }
            public ResourceConfig TesourceConfig { get { return resourceConfig; } }
        }

    }
}
