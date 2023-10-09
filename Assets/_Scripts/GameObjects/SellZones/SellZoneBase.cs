using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public abstract class SellZoneBase : TradeZone, ISellZone
	{     
        public Action OnAnyItemSoldEvent;
        public Action<VirtualCollectable> OnItemSoldEvent;

        protected override void OnTradeTrigger(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) OnStartSell(seller);
        }
        abstract protected void OnStartSell(ISeller seller);
        public bool TrySellItem(VirtualCollectable item)
        {
            if (!CanTrade) return false;
            if (i_TrySellItem(item))
            {
                OnAnyItemSoldEvent?.Invoke();
                OnItemSoldEvent?.Invoke(item);
                return true;
            }
            else return false;
        }
        virtual protected bool i_TrySellItem(VirtualCollectable item)
        {
            if (TradeToNowhere) return true;
            else
            {
                if (_hasStorage) return _storage.TryAdd(item);
                else return false;
            }
        }
    }
}
