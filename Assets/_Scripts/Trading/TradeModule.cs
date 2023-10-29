using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public abstract class TradeModule : MonoBehaviour
    {
        protected bool _isInTradeZone = false, _enoughGoodsForTrading = false, _storageCompositionChanged = false;
        protected int _colliderID;
        protected IStorage _storage;
        
        
        protected TradeContract _activeContract;

        protected Stack<VirtualCollectable> _preparedItemsList;
        public bool HasMultipleColliders => false;
        public int FreeSlotsCount => _storage.FreeSlotsCount;
        public int GetID() => _colliderID;
        public int[] GetIDs() => new int[1] { _colliderID };

        public TradeModule(int colliderID, IStorage storage)
        {
            _colliderID = colliderID;
            _storage = storage;
        }

        virtual protected void OnStorageCompositionChanged()
        {
            _storageCompositionChanged = true;
        }
        abstract public void Update();

    }
}
