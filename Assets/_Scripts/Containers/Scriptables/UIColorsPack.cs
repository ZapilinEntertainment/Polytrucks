using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {

    [CreateAssetMenu(menuName = "ScriptableObjects/UIColorsPack")]
    public sealed class UIColorsPack : ScriptableObject, IInitializable
	{
        [SerializeField] private CollectableIconColorsByTypePack _resourcesColors;
        [SerializeField] private RarityDefinedValues<Color> _colors;
        [SerializeField] private QuestTypeDefinedValues<Color> _questTypeMarkerColors;
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
        public Color GetQuestMarkerColor(QuestType type) => _questTypeMarkerColors[type];
        public Color GetResourceIconColor(CollectableType type, Rarity rarity = Rarity.Regular) => _resourcesColors.GetIconColor(type, rarity);
    }
}
