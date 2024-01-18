using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/IconsPack")]
    public sealed class IconsPack : ScriptableObject
    {
        [SerializeField] private Sprite _fruitsIcon, _ironIngotIcon, _lumberIcon, _steelBeamIngot, _woodenBeamIcon;

        public Sprite GetIcon(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.Fruits: return _fruitsIcon;
                case CollectableType.IronIngot: return _ironIngotIcon;
                case CollectableType.Lumber: return _lumberIcon;
                case CollectableType.SteelBeam: return _steelBeamIngot;
                case CollectableType.WoodenBeam: return _woodenBeamIcon;
                default: return null;
            }
        }
    }
}
