using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public sealed class SingleItemSellZone : SellZoneBase
    {
        [SerializeField] private CollectableType _itemType;
        [SerializeField] private RarityConditions _itemRarity;
        public CollectableType ItemType => _itemType;
        public RarityConditions RarityConditions => _itemRarity;

        public override TradeContract FormTradeContract() => new (_itemType.AsIntMaskValue(), FreeSlotsCount, _itemRarity);
    }
}
