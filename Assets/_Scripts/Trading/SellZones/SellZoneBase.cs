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
        protected IItemReceiver _itemsReceiver; // an object, to those this sell zone will send objects when received. Can be none - all goods will gone nowhere
        private EconomicSettings _economicSettings;

        public bool IsReadyToReceive => Time.time > _lastTradeTime + _tradeTick;
        virtual public int FreeSlotsCount => TradeToNowhere ? int.MaxValue : (_itemsReceiver?.FreeSlotsCount ?? 0);
        public Vector3 Position => transform.position;
        public Action OnAnyItemSoldEvent { get; set; }
        public Action<VirtualCollectable> OnItemSoldEvent { get; set; }

        [Inject]
        public void Inject(EconomicSettings economicSettings)
        {
            _economicSettings= economicSettings;
        }
        public void AssignReceiver(IItemReceiver receiver)
        {
            _itemsReceiver = receiver;
            _tradeToNowhere = receiver == null;
        }
        public override void AssignToZones(TradeZonesManager manager)
        {
            manager.AddSellZone(this);
        }

        protected override void OnTradeTriggerEnter(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) seller.OnEnterSellZone(this);
        }
        protected override void OnTradeTriggerExit(Collider other)
        {
            if (_collidersList.TryGetSeller(other.GetInstanceID(), out var seller)) seller.OnExitSellZone(this);
        }

        public bool TrySellItem(ISeller seller, VirtualCollectable item)
        {
            if (_isActive && (TradeToNowhere || (_itemsReceiver.TryAddItem(item))))
            {
                int cost = (int)(_economicSettings.GetCost(item.Rarity) * SellCostCf);
                seller.OnItemSold(new SellOperationContainer(cost, item.Rarity, Position));
                OnAnyItemSoldEvent?.Invoke();
                OnItemSoldEvent?.Invoke(item);
                _lastTradeTime = Time.time;
                return true;
            }
            else return false;
        }

        public void SellItems(IReadOnlyList<VirtualCollectable> list, out BitArray result)
        {
            if (!TradeToNowhere)
            {
                _itemsReceiver.AddItems(list, out result);               
            }
            else
            {
                result = new BitArray(list.Count, true);
            }
            if (OnItemSoldEvent != null)
            {
                foreach (var item in list)
                {
                    OnItemSoldEvent.Invoke(item);
                }
            }
            OnAnyItemSoldEvent?.Invoke();
        }

        public abstract TradeContract FormTradeContract();
       
       
    }
}
