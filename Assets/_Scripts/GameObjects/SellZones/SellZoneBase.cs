using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public abstract class SellZoneBase : TradeZone, ISellZone
	{
        virtual public float SellCostCf => 1f;

        protected override void OnTradeTrigger(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) _tradeSystem.MakeSell(seller, this);
        }

        public void SellItems(ICollection<VirtualCollectable> list)
        {
            if (!TradeToNowhere & _hasStorage) _storage.AddItems(list);
        }

        public abstract TradeContract FormTradeContract();
    }
}
