using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/IconsPack")]
    public sealed class IconsPack : ScriptableObject
    {
        [SerializeField] private Sprite _fruitsIcon, _metalsIcon, _lumberIcon;

        public Sprite GetIcon(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.Fruits: return _fruitsIcon;
                case CollectableType.Metals: return _metalsIcon;
                case CollectableType.Lumber: return _lumberIcon;
                default: return null;
            }
        }
    }
}
