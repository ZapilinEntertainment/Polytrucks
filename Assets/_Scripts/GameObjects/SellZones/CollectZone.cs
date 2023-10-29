using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CollectZone : TradeZone
    {
        public System.Action OnItemsCollectedEvent, OnItemAddedEvent;

        private bool TryDefineAsCollector(Collider collider, out ICollector collector) => _collidersList.TryGetCollector(collider.GetInstanceID(), out collector);

        public override void AssignStorage(IStorage storage)
        {
            if (_storage != null && _storage != storage)
            {
                _storage.OnItemAddedEvent -= OnStorageItemAdded;
            }
            base.AssignStorage(storage);
            _storage.OnItemAddedEvent += OnStorageItemAdded;
        }
        private void OnStorageItemAdded() => OnItemAddedEvent?.Invoke();

        protected override void OnTradeTriggerEnter(Collider other)
        {
           if (TryDefineAsCollector(other, out var collector))
            {
                collector.OnStartCollect(this);
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (TryDefineAsCollector(other, out var collector))
            {
                collector.OnStopCollect(this);
            }
        }

        public void ReturnItem(VirtualCollectable item) => _storage.TryAdd(item);
        public bool TryCollect(VirtualCollectable item) => _storage.TryExtract(item);
        public bool TryFormCollectionList(TradeContract contract, out List<VirtualCollectable> list)
        {
            if (_hasStorage) return _storage.TryFormItemsList(contract, out list);
            else
            {
                list = null;
                return false;
            }
        }
        public void RemoveItems(ICollection<VirtualCollectable> list)
        {
            if (_hasStorage)
            {
                _storage.RemoveItems(list);
                OnItemsCollectedEvent?.Invoke();
            }
        }
    }
}
