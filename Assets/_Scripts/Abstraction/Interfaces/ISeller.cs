using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISeller : IColliderOwner
	{
		public void OnEnterSellZone(ISellZone zone);
		public void OnExitSellZone(ISellZone zone);

		public bool TryStartSell(TradeContract contract, out List<VirtualCollectable> list);
		public void RemoveItems(ICollection<VirtualCollectable> list);
		public void OnItemSold(SellOperationContainer sellInfo);
	}
}
