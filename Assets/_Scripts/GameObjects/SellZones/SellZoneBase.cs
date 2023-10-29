using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public abstract class SellZoneBase : TradeZone, ISellZone
	{
        [field:SerializeField] virtual public float SellCostCf { get; private set; } = 1f;
        [SerializeField] protected float _tradeTick = 0.25f;
        protected float _lastTradeTime = 0f;
        private EconomicSettings _economicSettings;

        public bool IsReadyToReceive => Time.time > _lastTradeTime + _tradeTick;
        public Vector3 Position => transform.position;
        public Action OnAnyItemSoldEvent { get; set; }
        public Action<VirtualCollectable> OnItemSoldEvent { get; set; }

        [Inject]
        public void Inject(EconomicSettings economicSettings)
        {
            _economicSettings= economicSettings;
        }

        protected override void OnTradeTriggerEnter(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) seller.OnEnterSellZone(this);
        }
        protected override void OnTriggerExit(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) seller.OnExitSellZone(this);
        }

        public bool TrySellItem(ISeller seller, VirtualCollectable item)
        {
            if (TradeToNowhere || (_hasStorage && _storage.TryAdd(item)))
            {
                int cost = (int)(_economicSettings.GetCost(item.Rarity) * SellCostCf);
                seller.OnItemSold(new SellOperationContainer(cost, item.Rarity, Position));
                OnItemSoldEvent?.Invoke(item);
                _lastTradeTime = Time.time;
                return true;
            }
            else return false;
        }

        public void SellItems(ICollection<VirtualCollectable> list)
        {
            if (!TradeToNowhere & _hasStorage)
            {
                _storage.AddItems(list);               
            }
            if (OnItemSoldEvent != null)
            {
                foreach (var item in list)
                {
                    OnItemSoldEvent.Invoke(item);
                }
            }
        }

        public abstract TradeContract FormTradeContract();
       
       
    }
}
