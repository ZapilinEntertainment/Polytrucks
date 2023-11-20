using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class CollectionActivatedTrigger : MonoBehaviour, ICountTrackable
	{
		[SerializeField] private int _targetCount = 10;
		[SerializeField] private SellZoneBase _sellZone;
        [SerializeField] private MonoBehaviour _activableScript;
        [SerializeField] private CountTracker _countTracker;
        private bool _allItemsCollected = false;
        private int _itemsCollected = 0;
        private Action<bool> OnTrackStatusChangedEvent;
        private Action<int> OnCountChangedEvent;

        private void Start()
        {
            _sellZone.OnItemSoldEvent += OnItemSoldEvent;
            Subscribe(_countTracker);
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
                OnTrackStatusChangedEvent?.Invoke(false);

                (_activableScript as IActivableMechanism).Activate();
                
                Unsubscribe(_countTracker);
                _sellZone.Destroy();
                Destroy(this);
            }
        }

        public void Subscribe(ICountTracker tracker)
        {
            OnTrackStatusChangedEvent += tracker.OnTrackStatusChanged;
            OnCountChangedEvent += tracker.OnCountChanged;
            tracker.Setup(_targetCount, _itemsCollected, !_allItemsCollected);
        }
        public void Unsubscribe(ICountTracker tracker)
        {
            OnTrackStatusChangedEvent -= tracker.OnTrackStatusChanged;
            OnCountChangedEvent -= tracker.OnCountChanged;
            tracker.StopTracking();
        }
    }
}
