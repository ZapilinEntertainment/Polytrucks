using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class ReplenishableStorageBase : MonoBehaviour, IActivableMechanism
	{
		[SerializeField] private bool _isActive = true;
        [SerializeField] private int _startCount = 0;
		[SerializeField] private float _replenishTime = 15f;
        [SerializeField] private VirtualCollectable _spawningCollectable;

        private IItemReceiver _receiver;         
        private float _lastReplenishTime = 0f;
        public bool IsActive => _isActive;
        public System.Action OnActivatedEvent { get; set; }

        public void AssignReceiver(IItemReceiver receiver)
        {
            if (_receiver != null) UnsubscribeFromReceiver(receiver);
            _receiver = receiver;
            _receiver.SubscribeToItemRemoveEvent(OnStorageSlotEmptied);
            SetReceiverActivity(true);
            _lastReplenishTime = Time.time - _replenishTime;
        }
        public void UnsubscribeFromReceiver(IItemReceiver receiver)
        {
            _receiver.UnsubscribeFromItemRemoveEvent(OnStorageSlotEmptied);
            SetReceiverActivity(false);
            _receiver = null;
        }
        private void SetReceiverActivity(bool x)
        {
            var activable = _receiver as IActivitySwitchable;
            if (activable != null) activable.SetActivity(x);
        }

        virtual protected void Start() {
            if (_receiver != null) SpawnStartItems(_receiver);
        }
        protected void SpawnStartItems(IItemReceiver receiver)
        {
            if ( _startCount > 0)
            {
                int residue = receiver.AddItems(_spawningCollectable, _startCount);
                if (residue != 0) Debug.LogWarning($"{residue} items was not added");
            }
        }

        private void Update()
        {
            if (_isActive)
            {
                if (Time.time > _lastReplenishTime + _replenishTime)
                {
                    if (_receiver.TryAddItem(_spawningCollectable))
                    {
                        _lastReplenishTime = Time.time;
                    }
                    else
                    {
                        _isActive = false;
                    }
                }
            }
        }
        private void OnStorageSlotEmptied()
        {
            if (!_isActive) _isActive = true;
        }
        public void SetActivity(bool x)
        {
            _isActive = x;
            if (_receiver != null) SetReceiverActivity(x);
            if (_isActive) OnActivatedEvent?.Invoke();
        }
        public void Activate() => SetActivity(true);
    }
}
