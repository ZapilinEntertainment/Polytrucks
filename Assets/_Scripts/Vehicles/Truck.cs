using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public enum TruckID : byte { Undefined, TractorRosa, TruckRobert, RigCosetta, CarInessa, PickupCortney}
    public class Truck : Vehicle, ITrailerConnectionPoint
    {
        [SerializeField] private MassChanger _massChanger;
        [SerializeField] private AxisControllerBase _axisController;
        [SerializeField] private TruckConfig _truckConfig;        
        [SerializeField] private VehicleStorageController _storageController;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        private bool _haveTrailers = false;
        protected IStorage _storage;
        private TruckEngine _engine;
        private SellModule _sellModule;
        private CollectModule _collectModule;
        private ColliderListSystem _colliderListSystem;
        private List<Trailer> _trailers;
        private Joint _lock;

        override public float SteerValue => _engine.SteerValue;
        override public float GasValue => _engine.GasValue;
        public override float Speed => _axisController.Speed;
        public override float SpeedPc => _axisController.Speed / _truckConfig.MaxSpeed;
        public override Vector3 Position => _axisController.Position;
        public override VirtualPoint FormVirtualPoint() => new VirtualPoint() { Position = _axisController.Position, Rotation = _axisController.Rotation };
        public VirtualPoint CalculateTrailerPosition(float distance)
        {
            var rotation = _axisController.Rotation;
            return new VirtualPoint(_axisController.Position + rotation * (distance * Vector3.back), rotation);
        }

        public TruckConfig TruckConfig => _truckConfig;        
        public Action<IStorage> OnStorageChangedEvent;        

        [Inject]
        public void Inject(ColliderListSystem collidersList) => _colliderListSystem= collidersList;

        private void Awake()
        {
            Rigidbody.mass = _truckConfig.Mass; 
            UpdateStorageLink();
            _storageController.OnVehicleStorageCompositionChangedEvent += OnStorageCompositionChanged;
        }
        private void OnStorageCompositionChanged()
        {
            VehicleController.OnItemCompositionChanged();
        }
        private void Start()
        {
            _sellModule = new SellModule(_collidersHandler, _colliderListSystem, _storageController, this);
            _collectModule = new CollectModule(_collidersHandler, _colliderListSystem, _storageController, TruckConfig.CollectTime);

            _engine = new TruckEngine(_truckConfig, _axisController);            
            _axisController.Setup(this);            
        }
        private void UpdateStorageLink()
        {
            _storage = _storageController.Storage;
            if (_massChanger != null) _massChanger.Setup(_storageController.MainStorage);
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

        #region reposition
        public override void Teleport(VirtualPoint point) => _axisController.Teleport(point);
        public override void Stabilize() => _axisController.Stabilize();
        public override void RecoveryAt(RecoveryPoint point)
        {
            RemoveAllTrailers();
            _storage.MakeEmpty();
            Teleport(point.GetPoint());
        }
        public override void PhysicsLock(Rigidbody point)
        {
            if (_lock != null) Destroy(_lock);
            _lock = Rigidbody.gameObject.AddComponent<FixedJoint>();
            _lock.connectedBody = point;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.ResetInertiaTensor();
        }
        public override void PhysicsUnlock(Rigidbody point = null)
        {
            if (_lock != null && ( point == null || _lock.connectedBody == point))
            {
                Destroy(_lock);
                _lock = null;
            }
        }
        #endregion

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

        public void AddTrailer(Trailer trailer)
        {
            if (_trailers == null) _trailers = new List<Trailer>();
            _haveTrailers = true;
            _trailers.Add(trailer);
           // _collidersHandler.AddCollider(trailer.Collider);

            if (_storageController is SingleVehicleStorage)
            {
                var host = _storageController.gameObject;
                var cachedSettings = (_storageController as SingleVehicleStorage).StorageSettings;
                Destroy(_storageController);
                var multipleStorage = host.AddComponent<MultipleVehicleStorage>();
                multipleStorage.Setup(new StorageVisualSettings[1] { cachedSettings });
                multipleStorage.AddStorage(trailer.GetStorage());
                _storageController = multipleStorage;
                _storage = multipleStorage.Storage;
                UpdateStorageLink();
                OnStorageChangedEvent?.Invoke(_storage);
            }
            else
            {
                if (_storageController == null) _storageController = gameObject.AddComponent<MultipleVehicleStorage>();
                (_storageController as MultipleVehicleStorage).AddStorage(trailer.GetStorage());
            }

            int count = _trailers.Count;
            if (count == 1) trailer.OnTrailerConnected(this);
            else trailer.OnTrailerConnected(_trailers[count - 2]);
        }
        public void RemoveTrailer()
        {
            if (!_haveTrailers) return;
            int count = _trailers?.Count ?? 0;
            count--;
            var trailer = _trailers[count];
            var storage = (_storageController as MultipleVehicleStorage);
            if (storage != null)
            {
                if (trailer.IsStorageCreated) storage.RemoveStorage(trailer.GetStorage());
            }

             Destroy(trailer.gameObject);
            _trailers.RemoveAt(count);
            _haveTrailers = count != 0;
        }
        private void RemoveAllTrailers()
        {
            if (!_haveTrailers) return;
            var storage = (_storageController as MultipleVehicleStorage);
            if (storage != null)
            {
                int count = _trailers.Count;
                for (int i = 0; i < count; i++)
                {
                    var trailer = _trailers[i];
                    if (trailer.IsStorageCreated) storage.RemoveStorage(trailer.GetStorage());
                    Destroy(trailer.gameObject);
                }                
            }           

            _trailers.Clear();
            _haveTrailers = false;
        }
        public override IReadOnlyCollection<Vector3> GetVehicleBounds()
        {
            if (_haveTrailers)
            {
                var list = new List<Vector3>(base.GetVehicleBounds());
                foreach (var trailer in _trailers)
                {
                    var collider = trailer.Collider;
                    if (collider != null)
                    {
                        list.Add(collider.bounds.min);
                        list.Add(collider.bounds.max);
                    }
                }
                return list;
            }
            else return base.GetVehicleBounds();
        }

        public override void ClearCargo(bool destroy = true)
        {
            _storage.MakeEmpty();
        }
        public override TradeContract FormCollectContract() => _collectModule.FormCollectContract();
        public override bool CanFulfillContract(TradeContract contract) => _storage.CanFulfillContract(contract);
        public override int LoadCargo(VirtualCollectable item, int count) => _storage.AddItems(item, count);
        public override bool TryLoadCargo(VirtualCollectable item, int count) => _storage.TryLoadCargo(item, count);
    }
}
