using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks
{
    public enum CustomLayermask : byte { Default, }
    public enum DefinedLayer : byte { Default}

    public static class GameConstants
    {
        public const float GROUND_HEIGHT = 0f;

        public const string  DEFAULT_LAYERNAME = "Default";
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
                    default: value = LayerMask.GetMask(DEFAULT_LAYERNAME); break;
                }
                _customLayermasks.Add(customLayer, value);
            }
            return value;
        }
    }
}