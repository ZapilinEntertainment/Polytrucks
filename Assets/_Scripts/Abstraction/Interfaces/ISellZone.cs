using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISellZone
	{
        public float SellCostCf { get; }
        public void SellItems(ICollection<VirtualCollectable> list);
        public Vector3 Position { get; }
        public TradeContract FormTradeContract();

    }
}
