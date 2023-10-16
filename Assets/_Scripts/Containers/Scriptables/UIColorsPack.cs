using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/UIColorsPack")]
    public sealed class UIColorsPack : ScriptableObject, IInitializable
	{
        [Serializable] private class RarityColorsPack
        {
            [field:SerializeField] public Color Regular { get; private set;} = Color.white;
            [field: SerializeField] public Color Advanced { get; private set; } = Color.green;
            [field: SerializeField] public Color Industrial { get; private set; } = Color.blue;
            [field: SerializeField] public Color Rare { get; private set; } = Color.cyan;
            [field: SerializeField] public Color Mastery { get; private set; } = new Color(0.8509169f, 0.09905658f, 1f);
            [field: SerializeField] public Color Legendary { get; private set; } = new Color(1f, 0.5f, 1f);
            [field: SerializeField] public Color Unique { get; private set; } = Color.yellow;
        }

        [SerializeField] private RarityColorsPack _colorsPack;
        private Dictionary<Rarity, Color> _rarityColors ;

        [Inject]
        public void Inject(InitializableManager manager)
        {
            manager.Add(this);
        }

        public void Initialize()
        {
            _rarityColors = new Dictionary<Rarity, Color>()
            {
                {Rarity.Regular, _colorsPack.Regular }, {Rarity.Advanced, _colorsPack.Advanced}, {Rarity.Industrial, _colorsPack.Industrial},
                {Rarity.Rare, _colorsPack.Rare}, {Rarity.Mastery, _colorsPack.Mastery}, {Rarity.Legendary, _colorsPack.Legendary},
                {Rarity.Unique, _colorsPack.Unique}
            };
        }
        public Color GetRarityColor(Rarity rarity) => _rarityColors[rarity];
    }
}
