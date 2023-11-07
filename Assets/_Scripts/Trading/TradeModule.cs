using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public abstract class TradeModule
    {
        protected bool _isInTradeZone = false, _enoughGoodsForTrading = false, _storageCompositionChanged = false;
        protected HashSet<int> _activeColliders = new HashSet<int>();
        protected VehicleStorageController _storageController;
        protected IStorage Storage => _storageController.Storage;
        protected TradeCollidersHandler _collidersHandler;
        protected ColliderListSystem _colliderListSystem;
        
        protected TradeContract _activeContract;

        protected Stack<VirtualCollectable> _preparedItemsList;
        public bool HasMultipleColliders => _collidersHandler.HasMultipleColliders;
        public int FreeSlotsCount => Storage.FreeSlotsCount;
        public int GetID() => _collidersHandler.GetID();
        public int[] GetIDs() => _collidersHandler.GetIDs();

        public TradeModule(TradeCollidersHandler collidersHandler, ColliderListSystem colliderListSystem, VehicleStorageController storageController)
        {
            _collidersHandler = collidersHandler;
            _colliderListSystem = colliderListSystem;
            _collidersHandler.OnCollidersListChangedEvent += OnColliderListChanged;
            _storageController = storageController;
        }

        virtual protected void OnStorageCompositionChanged()
        {
            _storageCompositionChanged = true;
        }
        abstract public void Update();
        abstract protected void OnColliderListChanged();
    }
}
