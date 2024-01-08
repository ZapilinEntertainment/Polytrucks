using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class CollectionActivatedTrigger : MonoBehaviour, ICountTrackable
	{
		[SerializeField] private int _targetCount = 10;
		[SerializeField] private SingleItemSellZone _sellZone;
        [SerializeField] private MonoBehaviour _activableScript;
        [SerializeField] private CountTracker _countTracker;
        private bool _allItemsCollected = false;
        private int _itemsCollected = 0;
        private Action<int> OnCountChangedEvent;
        public int TargetCount => _targetCount;
        public int CollectedCount => _itemsCollected;
        public Vector3 Position => _sellZone.Position;
        public CollectableType ItemType => _sellZone.ItemType;
        public RarityConditions RarityConditions => _sellZone.RarityConditions;
        public Action OnConditionCompletedEvent;

        private void Start()
        {
            _sellZone.OnItemSoldEvent += OnItemSoldEvent;
            if (_countTracker != null) _countTracker.SetTrackingObject(this);
        }
        private void OnItemSoldEvent(VirtualCollectable item)
        {
            if (_allItemsCollected) return;
            _itemsCollected++;
            OnCountChangedEvent?.Invoke(_itemsCollected);
            if (_itemsCollected >= _targetCount)
            {
                _allItemsCollected = true;
                _sellZone.OnItemSoldEvent -= OnItemSoldEvent;
                _sellZone.SetActivity(false);

                (_activableScript as IActivableMechanism).Activate();
                OnConditionCompletedEvent?.Invoke();

                if (_countTracker != null)
                {
                    Unsubscribe(_countTracker);
                    _countTracker.OnTrackableDisposed();                    
                }
                _sellZone.Destroy();
                Destroy(this);
            }
        }

        public void Subscribe(ICountTracker tracker)
        {            
            OnCountChangedEvent += tracker.OnCountChanged;
            tracker.OnCountChanged(CollectedCount);
        }
        public void Unsubscribe(ICountTracker tracker)
        {
            OnCountChangedEvent -= tracker.OnCountChanged;
        }
    }
}
