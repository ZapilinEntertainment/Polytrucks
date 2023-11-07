using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public sealed class SingleVehicleStorage : VehicleStorageController
    {
        [SerializeField] private StorageVisualSettings _storageSettings;
        private Storage _storage;
        private StorageVisualizer _visualizer;
        public override IStorage Storage => _storage;
        public override Storage MainStorage => _storage;
        public StorageVisualSettings StorageSettings => _storageSettings;

        [Inject]
        public void Inject(StorageVisualizer.Factory visFactory)
        {
            _visualizer = visFactory.Create();
        }

        private void Awake()
        {
            _storage = new Storage(_storageSettings.Capacity);
        }
        private void Start()
        {            
            _visualizer.Setup(_storage, _storageSettings);
        }
    }
}
