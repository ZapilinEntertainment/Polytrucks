using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class TradeSystem
	{
		private Dictionary<Rarity, int> _basePrices = new Dictionary<Rarity, int>()
		{
			{ Rarity.Regular, 10}, {Rarity.Advanced, 25}, {Rarity.Industrial, 50},
			{Rarity.Rare, 100 }, {Rarity.Mastery, 250}, {Rarity.Legendary, 1000},
			{Rarity.Unique, 1000 }
		};

		public void MakeSell(ISeller seller, ISellZone sellzone)
		{
			var contract = sellzone.FormTradeContract();
			if (contract.IsValid())
			{
				List<VirtualCollectable> sellList;
				if (seller.TryStartSell(contract, out sellList))
				{
					seller.RemoveItems(sellList);
					sellzone.SellItems(sellList);

					float sellCostCf = sellzone.SellCostCf;
					foreach (var item in sellList)
					{
						seller.OnItemSold( 
							new SellOperationContainer() {
								MoneyCount =(int)(_basePrices[item.Rarity] * sellCostCf), 
								Rarity=	item.Rarity
							}
							);
					}
				}
			}
		}
		public void CollectFromStorage(CollectZone collectZone, ICollector collector)
		{
			var contract = collector.FormCollectContract();
			if (contract.IsValid())
			{
                List<VirtualCollectable> itemsList;
				if (collectZone.TryFromCollectionList(contract, out itemsList))
				{
					collectZone.RemoveItems(itemsList);
					collector.CollectItems(itemsList);
				}
            }
		}
	}
}
