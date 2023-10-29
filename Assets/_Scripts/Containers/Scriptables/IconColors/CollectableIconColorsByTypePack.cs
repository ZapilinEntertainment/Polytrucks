using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Collectables/ColorsByTypePack")]
    public sealed class CollectableIconColorsByTypePack : CollectableIconColorsPackBase
    {
        [System.Serializable]
        public struct CollectableTypeColor
        {
            public CollectableType Type;
            public Color Color;
        }
        [field: SerializeField] public List<CollectableTypeColor> Colors { get; private set; }
        public override bool CacheByRarity => false;

        public override Color GetIconColor(CollectableType collectableType, Rarity rarity)
        {
            TryGetIconColor(collectableType, out var color);
            return color;
        }
        public override Color GetIconColor(Rarity rarity) => DefaultColor;
        public override bool TryGetIconColor(CollectableType collectableType, out Color color)
        {
            foreach (var tuple in Colors)
            {
                if (tuple.Type == collectableType)
                {
                    color = tuple.Color;
                    return true;
                }
            }
            color = DefaultColor;
            return false;
        }
    }
}
