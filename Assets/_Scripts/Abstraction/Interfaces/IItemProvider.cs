using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IItemProvider
	{
		public int AvailableItemsCount { get; }

		public void SubscribeToProvisionListChange(Action action);
		public void UnsubscribeFromProvisionListChange(Action action);

		public void ReturnItem(VirtualCollectable item);
		public bool TryExtractItem(VirtualCollectable item);
		public bool TryExtractItems(VirtualCollectable item, int count);
		public bool TryExtractItems(TradeContract contract, out List<VirtualCollectable> list);

		public int CalculateItemsCount(CollectableType type, Rarity rarity);

    }
}
