using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class ReplenishableStorage : MonoBehaviour
	{
		[SerializeField] private bool _isActive = false;
        [SerializeField] private int _startCount = 0;
		[SerializeField] private float _replenishTime = 15f;
        [SerializeField] private VirtualCollectable _spawningCollectable;
        [SerializeField] private StorageVisualSettings _storageSettings;
        [SerializeField] private CollectZone _collectZone;
        
        private float _lastReplenishTime = 0f;
		private Storage _storage;
        private StorageVisualizer.Factory _visualizerFactory;

        [Inject]
        public void Inject(StorageVisualizer.Factory factory)
        {
            _visualizerFactory = factory;            
        }
        private void Awake()
        {
            _storage = new Storage(_storageSettings.Capacity);
            _storage.OnItemRemovedEvent += OnStorageSlotEmptied;
        }

        private void Start()
        {
            _visualizerFactory.Create().Setup(_storage, _storageSettings);

            if (_collectZone != null)
            {
                _collectZone.AssignItemsProvider(_storage); 
                _collectZone.SetActivity(_isActive);
            }
            if (_startCount > 0)
            {
                int residue = _storage.AddItems(_spawningCollectable, _startCount);
                if (residue != 0) Debug.Log($"{residue} items was not added");
            }
        }

        private void Update()
        {
            if (_isActive)
            {
                if (Time.time > _lastReplenishTime + _replenishTime)
                {
                    if (_storage.TryAddItem(_spawningCollectable))
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
