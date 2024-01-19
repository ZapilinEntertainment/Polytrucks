using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class SingleVehicleStorage : VehicleStorageController
    {
        [SerializeField] private StorageVisualSettings _storageSettings;
        private bool _storageSet = false;
        private Storage i_storage;
        private StorageVisualizer _visualizer;
        public override IStorage Storage => GetStorage();
        public override Storage MainStorage => GetStorage();
        public StorageVisualSettings StorageSettings => _storageSettings;

        private Storage GetStorage()
        {
            if (!_storageSet)
            {
                i_storage = new Storage(_storageSettings.Capacity);
                i_storage.OnStorageCompositionChangedEvent += OnVehicleStorageCompositionChangedEvent;
                _storageSet = true;
            }
            return i_storage;
        }

        [Inject]
        public void Inject(StorageVisualizer.Factory visFactory)
        {
            _visualizer = visFactory.Create();
        }

        private void Start()
        {            
            _visualizer.Setup(MainStorage, _storageSettings);
        }
    }
}
