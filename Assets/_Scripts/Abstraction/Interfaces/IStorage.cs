using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IStorage
	{
        public bool IsFull => ItemsCount == Capacity;
        public int ItemsCount { get; }
        public int FreeSlots { get; }
        public int Capacity { get; }        
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }

        public bool TryStartItemTransferTo(IStorage other);
        public bool TryAdd(VirtualCollectable collectable);
        public bool TryStartSell(ISellZone sellZone, int goodsMask, RarityConditions rarityConditions);        
        public bool TryExtract(CollectableType type, Rarity rarity);
        public bool TryExtract(CollectableType type, Rarity rarity, int count);

        public int CalculateItemsCountOfType(CollectableType type, Rarity rarity);
    }
}
