using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class CollectModule : TradeModule, ICollector

    {
        public bool IsReadyToReceive => Time.time - _lastReceiveTime > _receiveTime;
        protected float _receiveTime = 0.1f, _lastReceiveTime = - 1f;
        protected IVehicleController _controller;
        protected CollectZone _collectZone;
        public CollectModule(int colliderID, IStorage storage, float receiveTime) : base(colliderID, storage)
        {
            _receiveTime = receiveTime;
        }

        public void CollectItems(ICollection<VirtualCollectable> items)
        {
            _storage.AddItems(items);
        }     
        public void OnStartCollect(CollectZone zone)
        {
            if (_isInTradeZone) i_OnStopCollect();
            _collectZone = zone;
            _isInTradeZone = true;
            FormCollectContract();
            _collectZone.OnItemAddedEvent += OnStorageCompositionChanged;
        }
        protected void FormCollectContract()
        {
            _activeContract = new TradeContract(int.MaxValue, _storage.FreeSlotsCount, RarityConditions.Any);
            if (_activeContract.IsValid && _collectZone.TryFormCollectionList(_activeContract, out var list))
            {
                _preparedItemsList = new Stack<VirtualCollectable>(list);
                _enoughGoodsForTrading = _preparedItemsList.Count > 0;
            }
            else
            {
                _enoughGoodsForTrading = false;
            }
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
                _collectZone = null;
            }
            _isInTradeZone = false;
            _preparedItemsList.Clear();
            _enoughGoodsForTrading = false;
        }

        override public void Update()
        {
            if (_isInTradeZone)
            {
                if (IsReadyToReceive)
                {
                    if (_storageCompositionChanged)
                    {
                        FormCollectContract();
                    }
                    if (_enoughGoodsForTrading)
                    {
                        var item = _preparedItemsList.Pop();
                        if (_collectZone.TryCollect(item))
                        {
                            if (!TryReceive(item)) _collectZone.ReturnItem(item);
                        }
                        _enoughGoodsForTrading = _preparedItemsList.Count != 0;
                    }
                }
            }
        }

        public bool TryReceive(VirtualCollectable item) => _storage.TryReceive(item);
        public void ReceiveItems(ICollection<VirtualCollectable> items) => _storage.ReceiveItems(items);
    }
}
