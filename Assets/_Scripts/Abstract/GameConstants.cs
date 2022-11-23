using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public static class GameConstants 
    {
        private const string TERRAIN_LAYERNAME = "Terrain";

        public const float ITEM_SIZE = 1f;
        public const string TRUCK_TAG = "Truck";

        public static int GetTerrainLayermask() => LayerMask.GetMask(TERRAIN_LAYERNAME);
    }
}
