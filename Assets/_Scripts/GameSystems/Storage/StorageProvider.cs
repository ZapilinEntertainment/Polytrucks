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

        abstract public bool TryProvideItem(VirtualCollectable item);
        abstract public bool TryProvideItems(TradeContract contract, out List<VirtualCollectable> list);
        abstract public bool TryProvideItems(VirtualCollectable item, int count);

        abstract public int CalculateItemsCount(CollectableType type, Rarity rarity);
    }
}
