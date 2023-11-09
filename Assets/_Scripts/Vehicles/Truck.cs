using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
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

        override public float SteerValue => _engine.SteerValue;
        override public float GasValue => _engine.GasValue;
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
                storage.RemoveStorage(trailer.GetStorage());
            }

             Destroy(trailer.gameObject);
            _trailers.RemoveAt(count);
            _haveTrailers = count != 0;
        }

        public override TradeContract FormCollectContract() => _collectModule.FormCollectContract();
        public override bool CanFulfillContract(TradeContract contract) => _storage.CanFulfillContract(contract);
    }
}
