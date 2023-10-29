using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface ISellZone
	{
        public bool IsReadyToReceive { get; }
        public bool TrySellItem(ISeller seller, VirtualCollectable item);
        public void SellItems(ICollection<VirtualCollectable> list);
        public Vector3 Position { get; }
        public TradeContract FormTradeContract();
        public Action OnAnyItemSoldEvent { get; set; }
        public Action<VirtualCollectable> OnItemSoldEvent { get; set; }

    }
}
