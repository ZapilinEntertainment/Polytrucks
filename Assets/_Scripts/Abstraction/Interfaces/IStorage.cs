using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IStorage
	{
        public bool IsFull => ItemsCount == Capacity;
        public int ItemsCount { get; }
        public int FreeSlotsCount { get; }
        public int Capacity { get; }        
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }

        public void AddItems(ICollection<VirtualCollectable> items);
        public void RemoveItems(ICollection<VirtualCollectable> list);

        public bool TryFormItemsList(TradeContract contract, out List<VirtualCollectable> list);
        public bool TryAdd(VirtualCollectable collectable);
        public bool TryExtract(CollectableType type, Rarity rarity, int count);
        public int CalculateItemsCountOfType(CollectableType type, Rarity rarity);
    }
}
