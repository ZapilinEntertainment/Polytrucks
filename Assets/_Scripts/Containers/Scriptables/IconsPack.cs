using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/IconsPack")]
    public sealed class IconsPack : ScriptableObject
    {
        [SerializeField] private Sprite _fruitsIcon;

        public Sprite GetIcon(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.Fruits: return _fruitsIcon;
                default: return null;
            }
        }
    }
}
