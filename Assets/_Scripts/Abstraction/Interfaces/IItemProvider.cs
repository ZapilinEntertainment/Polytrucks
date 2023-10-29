using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IItemProvider
	{
		public void SubscribeToProvisionListChange(Action action);
		public void UnsubscribeFromProvisionListChange(Action action);
		public void ReturnItem(VirtualCollectable item);
		public bool TryProvideItem(VirtualCollectable item);
		public bool TryProvideItems(TradeContract contract, out List<VirtualCollectable> list);

    }
}
