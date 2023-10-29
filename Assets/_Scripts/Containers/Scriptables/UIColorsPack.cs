using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    [Serializable]
    public class RarityColorsPack
    {
        [field: SerializeField] public Color Regular { get; private set; } = Color.white;
        [field: SerializeField] public Color Advanced { get; private set; } = Color.green;
        [field: SerializeField] public Color Industrial { get; private set; } = Color.blue;
        [field: SerializeField] public Color Rare { get; private set; } = Color.cyan;
        [field: SerializeField] public Color Mastery { get; private set; } = new Color(0.8509169f, 0.09905658f, 1f);
        [field: SerializeField] public Color Legendary { get; private set; } = new Color(1f, 0.5f, 1f);
        [field: SerializeField] public Color Unique { get; private set; } = Color.yellow;

        public Color this[Rarity rarity]
        {
            get
            {
                switch(rarity)
                {
                    case Rarity.Unique: return Unique;
                    case Rarity.Legendary: return Legendary;
                    case Rarity.Mastery: return Mastery;
                    case Rarity.Rare: return Rare;
                    case Rarity.Industrial: return Industrial;
                    case Rarity.Advanced: return Advanced;
                    default: return Regular;
                }
            }
        }

        public Dictionary<Rarity, Color> ToDictionary() => new Dictionary<Rarity, Color>()
            {
                {Rarity.Regular, Regular }, {Rarity.Advanced, Advanced}, {Rarity.Industrial, Industrial},
                {Rarity.Rare, Rare}, {Rarity.Mastery, Mastery}, {Rarity.Legendary, Legendary},
                {Rarity.Unique, Unique}
            };
    }
    [Serializable]
    public class RarityDefinedValues<T>
    {
        [field: SerializeField] public T Regular { get; private set; } 
        [field: SerializeField] public T Advanced { get; private set; }
        [field: SerializeField] public T Industrial { get; private set; } 
        [field: SerializeField] public T Rare { get; private set; }
        [field: SerializeField] public T Mastery { get; private set; } 
        [field: SerializeField] public T Legendary { get; private set; } 
        [field: SerializeField] public T Unique { get; private set; }
        public T this[Rarity rarity]
        {
            get
            {
                switch (rarity)
                {
                    case Rarity.Unique: return Unique;
                    case Rarity.Legendary: return Legendary;
                    case Rarity.Mastery: return Mastery;
                    case Rarity.Rare: return Rare;
                    case Rarity.Industrial: return Industrial;
                    case Rarity.Advanced: return Advanced;
                    default: return Regular;
                }
            }
        }

        public Dictionary<Rarity, T> ToDictionary() => new Dictionary<Rarity, T>()
            {
                {Rarity.Regular, Regular }, {Rarity.Advanced, Advanced}, {Rarity.Industrial, Industrial},
                {Rarity.Rare, Rare}, {Rarity.Mastery, Mastery}, {Rarity.Legendary, Legendary},
                {Rarity.Unique, Unique}
            };
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/UIColorsPack")]
    public sealed class UIColorsPack : ScriptableObject, IInitializable
	{        
        [SerializeField] private RarityDefinedValues<Color> _colors;
        private Dictionary<Rarity, Color> _rarityColors ;

        [Inject]
        public void Inject(InitializableManager manager)
        {
            manager.Add(this);
        }

        public void Initialize()
        {
            _rarityColors = _colors.ToDictionary();
        }
        public Color GetRarityColor(Rarity rarity) => _rarityColors[rarity];
    }
}
