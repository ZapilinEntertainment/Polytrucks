using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public readonly struct TradeContract
	{
		public readonly int GoodsMask, MaxCount;
		public readonly RarityConditions RarityConditions;

		public TradeContract(int mask, int maxCount, RarityConditions rarity)
		{
			GoodsMask = mask;
			MaxCount = maxCount;
			RarityConditions = rarity;
		}

		public bool IsValid => MaxCount > 0 & GoodsMask != 0 & RarityConditions != 0;
		public bool IsItemSuits(VirtualCollectable item) => item.CollectableType.FitsInMask(GoodsMask) & RarityConditions.Contains(item.Rarity);
	}
}
