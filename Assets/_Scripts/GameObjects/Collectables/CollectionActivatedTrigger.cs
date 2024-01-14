using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
    public struct CompletedRequestReport
    {
        public readonly float ExperienceCollected;
        public readonly Vector3 Position;

        public CompletedRequestReport(CollectionActivatedTrigger trigger)
        {
            ExperienceCollected = trigger.CalculateExperienceReward();
            Position = trigger.Position;
        }
    }
	public sealed class CollectionActivatedTrigger : MonoBehaviour, ICountTrackable
	{
		[SerializeField] private int _targetCount = 10;
        [SerializeField] private float _experienceRewardCf = 1f;
		[SerializeField] private SingleItemSellZone _sellZone;
        [SerializeField] private MonoBehaviour _activableScript;
        [SerializeField] private CountTracker _countTracker;
        [SerializeField] private string _requireInfoStringID = string.Empty;
        private bool _allItemsCollected = false;
        private int _itemsCollected = 0;
        private RequestZonesManager _requestZonesManager;
        private Action<int> OnCountChangedEvent;
        public int TargetCount => _targetCount;
        public int CollectedCount => _itemsCollected;
        public Vector3 Position => _sellZone.Position;
        public CollectableType ItemType => _sellZone.ItemType;
        public RarityConditions RarityConditions => _sellZone.RarityConditions;
        public Action OnConditionCompletedEvent;
        public bool TryGetInfoString(out string stringID)
        {
            stringID = _requireInfoStringID;
            return _requireInfoStringID != string.Empty;
        }
        public float CalculateExperienceReward() => (((int)RarityConditions.MinimumRarity()) + 1) * _experienceRewardCf;

        [Inject]
        public void Inject(RequestZonesManager manager)
        {
            _requestZonesManager= manager;
        }

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
                OnConditionCompleted();
            }
        }
        private void OnConditionCompleted()
        {
            _allItemsCollected = true;
            _sellZone.OnItemSoldEvent -= OnItemSoldEvent;
            _sellZone.SetActivity(false);

            if (_countTracker != null)
            {
                Unsubscribe(_countTracker);
                _countTracker.OnTrackableDisposed();
            }
            _sellZone.Destroy();

            if (_activableScript != null) (_activableScript as IActivableMechanism).Activate();         
            
            OnConditionCompletedEvent?.Invoke();
            var finalReport = new CompletedRequestReport(this);
            _requestZonesManager.OnRequestZoneCompleted(finalReport);

            Destroy(this);
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
