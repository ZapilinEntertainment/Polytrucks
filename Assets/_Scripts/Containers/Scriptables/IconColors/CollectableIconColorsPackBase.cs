using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class CollectableIconColorsPackBase : ScriptableObject
	{
		abstract public bool CacheByRarity { get; }
		[field: SerializeField] public Color DefaultColor { get; protected set; } = Color.black;
		abstract public Color GetIconColor(CollectableType collectableType, Rarity rarity);
        abstract public Color GetIconColor(Rarity rarity);
        abstract public bool TryGetIconColor(CollectableType collectableType, out Color color);
    }
}
