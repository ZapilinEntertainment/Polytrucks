using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CityShop : SellZoneBase
    {
        public override TradeContract FormTradeContract() => new(int.MaxValue, FreeSlotsCount, RarityConditions.Any);
    }
}
