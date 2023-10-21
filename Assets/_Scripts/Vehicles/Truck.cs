using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class Truck : Vehicle,ICollector, ISeller
    {
        [SerializeField] private AxisControllerBase _axisController;
        [SerializeField] private TruckConfig _truckConfig;
        [SerializeField] private Collider _collectCollider;
        [SerializeField] private StorageVisualSettings _storageVisualSettings;
        private TruckEngine _engine;
        private StorageVisualizer _storageVisualizer;

        private Vector2 _moveDir = Vector2.up, _targetDir = Vector2.zero;
        private Vector2 RealDir
        {
            get
            {
                var fwd = _axisController?.Forward ?? transform.forward;
                return new Vector2(fwd.x, fwd.z).normalized;
            }
        }
        public float SteerValue => _engine.SteerValue;
        public float GasValue => _engine.GasValue;
        public override Vector3 Position => _axisController.Position;
        public override VirtualPoint FormVirtualPoint() => new VirtualPoint() { Position = _axisController.Position, Rotation = _axisController.Rotation };

        public TruckConfig TruckConfig => _truckConfig;

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
            collidersList.AddCollector(this);
            collidersList.AddSeller(this);
        }

        private void Start()
        {
            _moveDir = RealDir;

            _storage = new Storage(_storageVisualSettings.Capacity);
            _storageVisualizer.Setup(_storage, _storageVisualSettings);
            _engine = new TruckEngine(_truckConfig, _axisController);            
            _axisController.Setup();
        }
        
        private void Update()
        {
            if (!_truckConfig.UsesPhysics)
            {
                float t = Time.fixedDeltaTime;
                _engine.Update(t);
            }
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

        #region trading
        public TradeContract FormCollectContract() => new TradeContract(int.MaxValue, _storage.FreeSlotsCount, RarityConditions.Any);
        public void CollectItems(ICollection<VirtualCollectable> items) => _storage.AddItems(items);
        public bool TryStartSell(TradeContract contract, out List<VirtualCollectable> list) => _storage.TryFormItemsList(contract, out list);
        public void RemoveItems(ICollection<VirtualCollectable> list) => _storage.RemoveItems(list);

        public void OnItemSold(SellOperationContainer sellInfo) => _vehicleController?.OnItemSold(sellInfo);
        #endregion
    }
}
