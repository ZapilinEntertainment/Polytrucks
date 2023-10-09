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
                if (_hasStorage) collector.TryStartCollect(_storage);
                else if (TradeToNowhere) Debug.LogError("virtual storage is not configured");
            }
        }
    }
}
