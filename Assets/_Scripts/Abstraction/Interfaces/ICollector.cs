using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollector : IColliderOwner
	{
		public void OnStartCollect(CollectZone zone);
		public void OnStopCollect(CollectZone zone);
		public void CollectItems(ICollection<VirtualCollectable> items);
		public bool TryCollect(ICollectable collectable);
		public bool TryCollect(VirtualCollectable collectable);
    }
}
