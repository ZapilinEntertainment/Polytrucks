using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class Truck : Vehicle
    {
        [SerializeField] private AxisControllerBase _axisController;
        [SerializeField] private TruckConfig _truckConfig;
        [SerializeField] private Collider _collectCollider;
        [SerializeField] private StorageVisualSettings _storageVisualSettings;
        private TruckEngine _engine;
        private StorageVisualizer _storageVisualizer;
        private SellModule _sellModule;
        private CollectModule _collectModule;

        override public float SteerValue => _engine.SteerValue;
        override public float GasValue => _engine.GasValue;
        public override Vector3 Position => _axisController.Position;
        public override VirtualPoint FormVirtualPoint() => new VirtualPoint() { Position = _axisController.Position, Rotation = _axisController.Rotation };

        public TruckConfig TruckConfig => _truckConfig;
        public System.Action<float> OnCargoMassChangedEvent;

        #region icollector
        public bool HasMultipleColliders => false;
        public int GetID() => _collectCollider.GetInstanceID();
        public int[] GetIDs() => new int[] { _collectCollider.GetInstanceID() };
        public bool TryCollect(ICollectable collectable) => _storage.TryAdd(collectable.ToVirtual());
        #endregion

        [Inject]
        public void Inject(StorageVisualizer.Factory storageVisualizerFactory, ColliderListSystem collidersList)
        {
            _storageVisualizer = storageVisualizerFactory.Create();
            int colliderId = _collectCollider.GetInstanceID();

            _storage = new Storage(_storageVisualSettings.Capacity);
            _sellModule = new SellModule(colliderId, _storage,_vehicleController);
            _collectModule = new CollectModule(colliderId, _storage, TruckConfig.CollectTime);

            collidersList.AddSeller(_sellModule);
            collidersList.AddCollector(_collectModule);
        }

        private void Start()
        {            
            _storageVisualizer.Setup(_storage, _storageVisualSettings);
            _engine = new TruckEngine(_truckConfig, _axisController);            
            _axisController.Setup(this);
            _storage.OnStorageCompositionChangedEvent += OnStorageCompositionChanged;
        }
        
        private void Update()
        {
            if (!_truckConfig.UsesPhysics)
            {
                float t = Time.fixedDeltaTime;
                _engine.Update(t);
            }
            _sellModule.Update();
            _collectModule.Update();
        }
        private void FixedUpdate()
        {
            if (_truckConfig.UsesPhysics)
            {
                float t = Time.fixedDeltaTime;
                _engine.Update(t);
            }
        }

        public override void Teleport(VirtualPoint point) => _axisController.Teleport(point);
        public override void Stabilize() => _axisController.Stabilize();

        #region controls
        override public void Move(Vector2 dir)
        {

        }
        override public void Gas() => _engine.Gas();
        override public void Brake() => _engine.Brake();
        override public void Reverse() => _engine.Reverse();
        override public void ReleaseGas() => _engine.ReleaseControl();
        override public void Steer(float x)
        {
            _engine.SetSteer(x);
            //if (x == 0f) _axisController.Steer(0f);
        }


        #endregion
        
        private void OnStorageCompositionChanged() => OnCargoMassChangedEvent?.Invoke(_storage.CargoMass);
    }
}
