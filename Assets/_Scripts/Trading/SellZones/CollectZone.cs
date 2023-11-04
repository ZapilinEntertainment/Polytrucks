using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CollectZone : TradeZone
    {
        protected IItemProvider _itemProvider;
        public System.Action OnItemsCollectedEvent, OnItemAddedEvent;

        private bool TryDefineAsCollector(Collider collider, out ICollector collector) => _collidersList.TryGetCollector(collider.GetInstanceID(), out collector);

        public void AssignItemsProvider(IItemProvider provider)
        {
            if (_itemProvider != null && _itemProvider != provider)
            {
                _itemProvider.UnsubscribeFromProvisionListChange( OnStorageItemAdded);
            }
            _itemProvider = provider;
            _itemProvider.SubscribeToProvisionListChange(OnStorageItemAdded);
        }
        private void OnStorageItemAdded() => OnItemAddedEvent?.Invoke();

        protected override void OnTradeTriggerEnter(Collider other)
        {
           if (TryDefineAsCollector(other, out var collector))
            {
                collector.OnStartCollect(this);
            }
        }
        protected override void OnTradeTriggerExit(Collider other)
        {
            if (TryDefineAsCollector(other, out var collector))
            {
                collector.OnStopCollect(this);
            }
        }

        public void ReturnItem(VirtualCollectable item) => _itemProvider.ReturnItem(item);
        public bool TryCollect(VirtualCollectable item) => _itemProvider.TryExtractItem(item);
        public bool TryFormCollectionList(TradeContract contract, out List<VirtualCollectable> list) => _itemProvider.TryExtractItems(contract, out list);
    }
}
