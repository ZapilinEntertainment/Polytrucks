using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollector : IColliderOwner, IItemReceiver
	{
		public void OnStartCollect(CollectZone zone);
		public void OnStopCollect(CollectZone zone);
		public void CollectItems(ICollection<VirtualCollectable> items) => ReceiveItems(items);
		public bool TryCollect(ICollectable collectable) => TryReceive(collectable.ToVirtual());
    }
}
