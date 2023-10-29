using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Collectables/ColorsByRarityPack")]
    public sealed class CollectableIconColorsByRarityPack : CollectableIconColorsPackBase
    {
        [field: SerializeField] public RarityColorsPack Colors { get; private set; }
        public override bool CacheByRarity => true;
        

        public override Color GetIconColor(CollectableType collectableType, Rarity rarity) => Colors[rarity];
        public override Color GetIconColor(Rarity rarity) => Colors[rarity];
        public override bool TryGetIconColor(CollectableType collectableType, out Color color)
        {
            color = DefaultColor;
            return false;
        }
    }

}
