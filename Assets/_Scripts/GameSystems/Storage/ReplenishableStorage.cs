using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class ReplenishableStorage : MonoBehaviour
	{
		[SerializeField] private bool _isActive = false;
		[SerializeField] private float _replenishTime = 15f;
        [SerializeField] private VirtualCollectable _spawningCollectable;
        [SerializeField] private StorageVisualSettings _storageSettings;
        [SerializeField] private CollectZone _collectZone;
        
        private float _lastReplenishTime = 0f;
		private IStorage _storage;

        [Inject]
        public void Inject(Storage.Factory storageFactory)
        {
            _storage = storageFactory.Create(_storageSettings);
            _storage.OnItemRemovedEvent += OnStorageSlotEmptied;
            _collectZone.AssignStorage(_storage);
        }
        private void Start()
        {
            if (_collectZone != null) _collectZone.SetActivity(_isActive);
        }

        private void Update()
        {
            if (_isActive)
            {
                if (Time.time > _lastReplenishTime + _replenishTime)
                {
                    if (_storage.TryAdd(_spawningCollectable))
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
            if (_collectZone != null) _collectZone.SetActivity(_isActive);
            Debug.Log(Time.time > _lastReplenishTime + _replenishTime);
        }
    }
}
