using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public abstract class SellZoneBase : TradeZone, ISellZone
	{
        [field:SerializeField] virtual public float SellCostCf { get; private set; } = 1f;
        public Vector3 Position => transform.position;
        public Action OnItemSoldEvent { get; set; }

        protected override void OnTradeTrigger(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) _tradeSystem.MakeSell(seller, this);
        }

        public void SellItems(ICollection<VirtualCollectable> list)
        {
            if (!TradeToNowhere & _hasStorage)
            {
                _storage.AddItems(list);               
            }
            OnItemSoldEvent?.Invoke();
        }

        public abstract TradeContract FormTradeContract();
    }
}
