using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class CollectZone : TradeZone
    {
        protected override void OnTradeTrigger(Collider other)
        {
           if (_collidersList.TryGetCollector(other.GetInstanceID(), out var collector))
            {
                if (_hasStorage) _tradeSystem.CollectFromStorage(this, collector);
                else if (TradeToNowhere) Debug.LogError("virtual storage is not configured");
            }
        }
        public bool TryFromCollectionList(TradeContract contract, out List<VirtualCollectable> list)
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
            if (_hasStorage) _storage.RemoveItems(list);
        }
    }
}
