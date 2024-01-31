using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public abstract class TradeModule
    {
        protected bool _isInTradeZone = false, _enoughGoodsForTrading = false, _storageCompositionChanged = false, _isDisposed = false;
        protected HashSet<int> _activeColliders = new HashSet<int>();
        protected IStorageController _storageController;
        protected IStorage Storage => _storageController.Storage;
        protected CollidersHandler _collidersHandler;
        protected ColliderListSystem _colliderListSystem;
        
        protected TradeContract _activeContract;

        protected Stack<VirtualCollectable> _preparedItemsList;
        public bool HaveMultipleColliders => _collidersHandler.HaveMultipleColliders;
        public int FreeSlotsCount => Storage.FreeSlotsCount;
        public int GetColliderID() => _collidersHandler.GetColliderID();
        public int[] GetColliderIDs() => _collidersHandler.GetColliderIDs();

        public TradeModule(CollidersHandler collidersHandler, ColliderListSystem colliderListSystem, IStorageController storageController)
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

        protected void Dispose()
        {
            if (!_isDisposed)
            {
                OnDispose();
                _isDisposed = true;
                if (_collidersHandler != null)
                {
                    _collidersHandler.OnCollidersListChangedEvent-= OnColliderListChanged;
                }
            }
        }
        abstract protected void OnDispose();
    }
}
