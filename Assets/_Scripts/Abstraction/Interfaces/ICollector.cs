using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollector : IColliderOwner
	{
		public void CollectItems(ICollection<VirtualCollectable> items);
		public bool TryCollect(ICollectable collectable);
		public TradeContract FormCollectContract();
    }
}
