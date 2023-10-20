using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/IconsPack")]
    public sealed class IconsPack : ScriptableObject
    {
        [SerializeField] private Sprite _fruitsIcon, _metalsIcon;

        public Sprite GetIcon(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.Fruits: return _fruitsIcon;
                case CollectableType.Metals: return _metalsIcon;
                default: return null;
            }
        }
    }
}
