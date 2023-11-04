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
        [SerializeField] private VehicleStorageController _storageController;
        private TruckEngine _engine;
        private SellModule _sellModule;
        private CollectModule _collectModule;
        private ColliderListSystem _collidersList;

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
        public bool TryCollect(ICollectable collectable) => _storage.TryAddItem(collectable.ToVirtual());
        #endregion

        [Inject]
        public void Inject(ColliderListSystem collidersList)
        {
            _collidersList = collidersList;            
        }

        private void Start()
        {
            int colliderId = _collectCollider.GetInstanceID();

            _storage = _storageController.Storage;
            _storageController.OnVehicleCargoChangedEvent += OnCargoMassChangedEvent;

            _sellModule = new SellModule(colliderId, _storage, this);
            _collectModule = new CollectModule(colliderId, _storage, TruckConfig.CollectTime);

            _collidersList.AddSeller(_sellModule);
            _collidersList.AddCollector(_collectModule);

            _engine = new TruckEngine(_truckConfig, _axisController);            
            _axisController.Setup(this);
            
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
        
    }
}
