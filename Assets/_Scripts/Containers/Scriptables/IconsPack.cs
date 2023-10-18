using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/IconsPack")]
    public sealed class IconsPack : ScriptableObject
    {
        [SerializeField] private Sprite _fruitsIcon, _metalsIcon;
        private bool _dictionaryFormed = false;
        private Dictionary<CollectableType, Sprite> _icons;

        public Sprite GetIcon(CollectableType type)
        {
            if (!_dictionaryFormed)
            {
                _icons = new Dictionary<CollectableType, Sprite>()
                {
                    {CollectableType.Fruits, _fruitsIcon }, {CollectableType.Metals, _metalsIcon}
                };
                _dictionaryFormed = true;
            }
            return _icons[type];
        }
    }
}
