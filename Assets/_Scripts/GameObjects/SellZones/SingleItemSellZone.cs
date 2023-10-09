using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public sealed class SingleItemSellZone : SellZoneBase
    {
        [SerializeField] private CollectableType _itemType;
        [SerializeField] private RarityConditions _itemRarity;

        protected override void OnStartSell(ISeller seller)
        {
            seller.TryStartSell(this, _itemType.AsIntMaskValue(), _itemRarity);
        }
    }
}
