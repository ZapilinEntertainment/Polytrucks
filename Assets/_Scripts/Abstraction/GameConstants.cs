using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    public enum CustomLayermask : byte { Default, Tyres, VehicleDamageMask}
    public enum DefinedLayer : byte { Default, Collectible, Player, Terrain, DirtZone}

    public static class GameConstants
    {
        public const float GROUND_HEIGHT = 0f, CRATE_COLLECT_RADIUS = 1f, DEFAULT_COLLECTABLE_SIZE = 1.5f, BASE_CRATE_MASS = 5f, MAX_SPEED = 100f,
            WALL_HIT_DAMAGE_CF = 0.01f;

        public const string  DEFAULT_LAYERNAME = "Default", COLLECTIBLE_LAYERNAME = "Collectible", PLAYER_LAYERNAME = "Player", TERRAIN_LAYERNAME = "Terrain", DIRTZONE_LAYERNAME = "DirtZone";

        private static Dictionary<CustomLayermask, int> _customLayermasks = new Dictionary<CustomLayermask, int>();
        private static Dictionary<DefinedLayer, int> _definedLayers = new Dictionary<DefinedLayer, int>();

        public static int GetDefinedLayer(DefinedLayer definedLayer)
        {
            int layer = 0;
            if (!_definedLayers.TryGetValue(definedLayer, out layer))
            {
                string layerName ;
                switch (definedLayer)
                {
                    case DefinedLayer.Player: layerName = PLAYER_LAYERNAME; break;
                    case DefinedLayer.Collectible: layerName = COLLECTIBLE_LAYERNAME;break;
                    case DefinedLayer.Terrain: layerName = TERRAIN_LAYERNAME; break;
                    case DefinedLayer.DirtZone: layerName = DIRTZONE_LAYERNAME; break;
                    default: layerName = DEFAULT_LAYERNAME; break;
                }
                layer = LayerMask.NameToLayer(layerName);
                _definedLayers.Add(definedLayer, layer);
            }
            return layer;
        }
        public static int GetCustomLayermask(CustomLayermask customLayer)
        {
            if (!_customLayermasks.TryGetValue(customLayer, out int value))
            {
                switch (customLayer)
                {
                    case CustomLayermask.VehicleDamageMask: value = LayerMask.GetMask(DEFAULT_LAYERNAME, TERRAIN_LAYERNAME); break;
                    case CustomLayermask.Tyres: value = LayerMask.GetMask(DEFAULT_LAYERNAME, TERRAIN_LAYERNAME, DIRTZONE_LAYERNAME); break;
                    default: value = LayerMask.GetMask(DEFAULT_LAYERNAME); break;
                }
                _customLayermasks.Add(customLayer, value);
            }
            return value;
        }
    }
}
