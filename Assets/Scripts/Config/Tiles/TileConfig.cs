using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using Untitled.Tiles;

namespace Untitled
{
    namespace Configs
    {

        [CreateAssetMenu(fileName = "TileConfig", menuName = "Config/Tiles")]
        public class TileConfig : ScriptableObject
        {
            [SerializeField] private int startingCoalValue;
            [SerializeField] private TileType[] resourceTiles;

            public int StartingCoalValue { get {return startingCoalValue; } }
            public TileType[] ResourceTiles { get { return resourceTiles; } }

            private Dictionary<TileType, int> tileStartingValues;
            public Dictionary<TileType, int> TileStartingValues
            {
                get
                {
                    if (tileStartingValues == null)
                        tileStartingValues = new Dictionary<TileType, int>()
                        {
                            { TileType.Coal, startingCoalValue }
                        };
                    return tileStartingValues;
                }
            }
        }

    }
}
