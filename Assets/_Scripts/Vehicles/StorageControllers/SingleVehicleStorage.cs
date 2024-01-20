using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class SingleVehicleStorage : VehicleStorageController
    {
        [SerializeField] private Transform _zeroPoint;
        private bool _storageSet = false;
        private Storage i_storage;
        private StorageVisualizer _visualizer;
        private IStorageConfiguration _storageConfig;
        public override IStorage Storage => GetStorage();
        public override Storage MainStorage => GetStorage();

        public override void SetOnVehicleStorageConfig(VisualStorageSettings config)
        {
            _storageConfig = config.StorageConfiguration;
            if (_zeroPoint == null) _zeroPoint = config.ZeroPoint;            
        }
        private Storage GetStorage()
        {
            if (!_storageSet)
            {
                if (_storageConfig == null) _storageConfig = new VirtualStorageConfiguration();
                i_storage = new Storage(_storageConfig.Capacity);
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
            _visualizer?.Setup(MainStorage, new VirtualVisualStorageSettings(_storageConfig, _zeroPoint));
        }
    }
}
