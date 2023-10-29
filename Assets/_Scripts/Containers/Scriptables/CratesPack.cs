using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Collectables/CratesPack")]
    public class CratesPack : ScriptableObject
	{
        [field: SerializeField] public Crate CratePrefab { get; protected set; }
        [field: SerializeField] public CollectibleModel CrateModel { get; protected set; }
        [field : SerializeField] public CollectableIconColorsPackBase IconColorsPack { get; protected set; }
        [SerializeField] protected Mesh[] _crateMeshes;

        public Mesh GetCrateModel(Rarity rarity) => _crateMeshes[(int)rarity];
        public Cacher CreateCacher() => new Cacher(this);

        public class Cacher
        {
            private bool _cacheByRarity = false;
            private Color _defaultColor= Color.white;
            private Dictionary<int, Color> _iconColors;

            public Cacher(CratesPack crates)
            {

                var colorsPack = crates.IconColorsPack;
                _defaultColor = colorsPack.DefaultColor;
                _cacheByRarity = colorsPack.CacheByRarity;     
                _iconColors = new Dictionary<int, Color>();
                if (_cacheByRarity)
                {
                    var rarities = Enum.GetValues(typeof(Rarity));
                    foreach (Rarity val in rarities)
                    {
                        _iconColors.Add((int)val, colorsPack.GetIconColor(val));
                    }
                }
                else
                {
                    var collectableTypes = Enum.GetValues(typeof(CollectableType));
                    foreach (CollectableType type in collectableTypes)
                    {
                        if (colorsPack.TryGetIconColor(type, out Color color))
                        {
                            _iconColors.Add((int)type, color);
                        }
                    }
                }
            }
            public Color GetIconColor(CollectableType type, Rarity rarity)
            {
                int key = _cacheByRarity ? (int)rarity : (int)type;
                if (_iconColors.TryGetValue(key, out Color color)) return color;
                else
                {
                    return _defaultColor;
                }
            }
        }
    }
}
