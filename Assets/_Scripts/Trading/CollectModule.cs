using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class CollectModule : TradeModule, ICollector

    {
        public bool IsReadyToReceive => Time.time - _lastReceiveTime > _receiveTime;
        protected float _receiveTime = 0.1f, _lastReceiveTime = - 1f;
        protected Vehicle _vehicle;
        protected CollectZone _collectZone;
        public CollectModule(CollidersHandler collidersHandler, ColliderListSystem colliderListSystem, Vehicle vehicle, float receiveTime) : base(collidersHandler, colliderListSystem, vehicle.VehicleStorageController)
        {
            _receiveTime = receiveTime;
            _vehicle = vehicle;
            _vehicle.OnVehicleDisposeEvent += Dispose;
            colliderListSystem?.AddCollector(this);
        }

        public TradeContract FormCollectContract() => new TradeContract(mask: int.MaxValue, maxCount: Storage.FreeSlotsCount, RarityConditions.Any);

        public bool TryCollect(ICollectable collectable) => _isDisposed ?false : Storage.TryAddItem(collectable.ToVirtual());
        public bool TryCollect(VirtualCollectable collectable) => _isDisposed ? false : Storage.TryAddItem(collectable);
        public void CollectItems(IList<VirtualCollectable> items, out BitArray result)
        {
            if (_isDisposed)
            {
                result = new BitArray(0);
                return;
            }
            else
            {
                Storage.AddItems(items, out result);
                _lastReceiveTime = Time.time;
            }
        }     
        public void OnStartCollect(CollectZone zone)
        {
            if (_isDisposed) return;
            if (_isInTradeZone) i_OnStopCollect();

            _collectZone = zone;
            if (_collectZone != null)
            {
                _isInTradeZone = true;
                PrepareCollectContractAndList();
                _collectZone.OnItemAddedEvent += OnStorageCompositionChanged;
                _collectZone.OnTradeZoneDisposedEvent += i_OnStopCollect;
            }
        }
        protected void PrepareCollectContractAndList()
        {
            _activeContract = FormCollectContract();
            if (_activeContract.IsValid && _collectZone.TryFormCollectionList(_activeContract, out var list))
            {
                _preparedItemsList = new Stack<VirtualCollectable>(list);
                _enoughGoodsForTrading = _preparedItemsList.Count > 0;
            }
            else
            {
                _enoughGoodsForTrading = false;
            }
            _storageCompositionChanged = false;
        }

        public void OnStopCollect(CollectZone zone)
        {
            if (_isInTradeZone && _collectZone == zone) i_OnStopCollect();
        }
        private void i_OnStopCollect()
        {
            if (_collectZone != null)
            {
                _collectZone.OnItemAddedEvent -= OnStorageCompositionChanged;
                _collectZone.OnTradeZoneDisposedEvent -= i_OnStopCollect;
                _collectZone = null;
            }
            _isInTradeZone = false;
            _preparedItemsList?.Clear();
            _enoughGoodsForTrading = false;
        }

        override public void Update()
        {
            if (_isInTradeZone & !_isDisposed)
            {
                if (IsReadyToReceive)
                {
                    if (_storageCompositionChanged)
                    {
                        PrepareCollectContractAndList();
                    }
                    if (_enoughGoodsForTrading)
                    {
                        var item = _preparedItemsList.Pop();
                        if (_collectZone.TryCollect(item))
                        {
                            if (!TryCollect(item)) _collectZone.ReturnItem(item);
                        }
                        _enoughGoodsForTrading = _preparedItemsList.Count != 0;
                    }
                }
            }
        }

        protected override void OnColliderListChanged()
        {
            if (!_isDisposed)  _colliderListSystem.OnCollectorChanged(this);
        }

        protected override void OnDispose()
        {
            i_OnStopCollect();
            _colliderListSystem?.RemoveCollector(this);
            if (_vehicle != null) _vehicle.OnVehicleDisposeEvent -= Dispose;
        }
    }
}
