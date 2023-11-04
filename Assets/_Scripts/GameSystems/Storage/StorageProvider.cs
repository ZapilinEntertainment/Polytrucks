using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public abstract class StorageProvider : MonoBehaviour, IItemProvider
    {
        protected IStorage _storage;

        public int AvailableItemsCount => _storage?.AvailableItemsCount ?? 0;

        virtual public void AssignStorage(IStorage storage) => _storage = storage;


        abstract public void ReturnItem(VirtualCollectable item);
        abstract public void SubscribeToProvisionListChange(Action action);
        abstract public void UnsubscribeFromProvisionListChange(Action action);

        abstract public bool TryExtractItem(VirtualCollectable item);
        abstract public bool TryExtractItems(TradeContract contract, out List<VirtualCollectable> list);
        abstract public bool TryExtractItems(VirtualCollectable item, int count);

        abstract public int CalculateItemsCount(CollectableType type, Rarity rarity);
    }
}
