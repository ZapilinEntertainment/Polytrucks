using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
    public enum TruckID : byte { Undefined, TractorRosa, TruckRobert, RigCosetta, CarInessa, PickupCortney}
    public class Truck : Vehicle, IAxisController, ICachableVehicle, ITeleportable
    {
        [SerializeField] private MassChanger _massChanger;
        [SerializeField] private AxisControllerBase _axisController;
        [SerializeField] private TruckConfig _truckConfig;        
        [SerializeField] private StorageController _storageController;
        [field: SerializeField] public Rigidbody Rigidbody { get; private set; }
        private TruckEngine _engine;
        private SellModule _sellModule;
        private CollectModule _collectModule;
        private FuelModule _fuelModule;
        private IntegrityModule _integrityModule;
        private ColliderListSystem _colliderListSystem;        
        private Joint _physicsLock;
        private TrailerConnector.Handler _trailerConnectorHandler;

        protected IStorage Storage => _storageController.Storage;
        public override StorageController VehicleStorageController => _storageController;
        public TrailerConnector TrailerConnector => _trailerConnectorHandler.Connector;

        public bool IsBraking { get; private set; } = false;
        public bool HaveTrailers
        {
            get
            {
                if (_trailerConnectorHandler.IsActivated) return TrailerConnector.HaveTrailers;
                else return false;
            }
        }
        public override bool IsTeleporting => _axisController.IsTeleporting;
        override public float SteerValue => _engine.SteerValue;
        override public float GasValue => _engine.GasValue;
        public override float Speed => _axisController.Speed;
        public override float SpeedPc => _axisController.Speed / _truckConfig.MaxSpeed;
        public float MaxSteerAngle => _truckConfig.MaxSteerAngle;
        public float MaxEngineSpeed => _truckConfig.MaxSpeed;
        public float Passability => _truckConfig.Passability;
       
        public override Vector3 Position => _axisController.Position;
        public override VirtualPoint FormVirtualPoint() => new VirtualPoint() { Position = _axisController.Position, Rotation = _axisController.Rotation };
        public float CalculatePowerEffort(float pc) => _truckConfig.CalculatePowerEffort(pc);
        public override bool TryGetStorage(out IStorage storage)
        {
            storage = Storage;
            return storage != null;
        }
        public override bool TryGetFuelModule(out FuelModule module)
        {
            if (_truckConfig.UseFuel)
            {
                module = _fuelModule;
                return true;
            }
            else
            {
                module = null;
                return false;
            }
        }
        public override bool TryGetIntegrityModule(out IntegrityModule module)
        {
            if (_truckConfig.UseIntegrity)
            {
                module = _integrityModule;
                return true;
            }
            else
            {
                module = null;
                return false;
            }
        }

        public TruckID TruckID => TruckConfig.TruckID;
        public TruckConfig TruckConfig => _truckConfig;        
        public Action<IStorage> OnStorageChangedEvent;

        [Inject]
        public void Inject(ColliderListSystem collidersList, TrailerConnector.Handler.Factory trailerConnectorHandlerFactory)
        {
            _colliderListSystem = collidersList;
            _trailerConnectorHandler= trailerConnectorHandlerFactory.Create(this);
        }

        private void Awake()
        {
            Rigidbody.mass = _truckConfig.Mass;

            if (_truckConfig.UseFuel)
            {
                _fuelModule = new FuelModule(_truckConfig.FuelConfiguration, this);
                _engine = new FueledTruckEngine(_fuelModule, _truckConfig, _axisController);
            }
            else
            {
                _fuelModule = null;
                _engine = new TruckEngine(_truckConfig, _axisController);
            }
            if (_truckConfig.UseIntegrity)
            {
                _integrityModule = new IntegrityModule(_collidersHandler, _truckConfig.IntegrityConfiguration);
                _integrityModule.OnDestructedEvent += OnVehicleLoseIntegrity;
            }

            UpdateStorageLink();
            _storageController.OnStorageCompositionChangedEvent += OnStorageCompositionChanged;
        }
        private void OnStorageCompositionChanged()
        {
            VehicleController?.OnItemCompositionChanged();
        }
        private void Start()
        {
            _sellModule = new SellModule(_collidersHandler, _colliderListSystem,  this);
            _collectModule = new CollectModule(_collidersHandler, _colliderListSystem, this, TruckConfig.CollectTime);
                      
            _axisController.Setup(this);
        }
        private void UpdateStorageLink()
        {
            var storageConfig = _truckConfig.StorageConfiguration;
            if (storageConfig != null) {
                _storageController.SetInitialStorageConfig(new VisualStorageSettings(null, storageConfig)); // #problem
                if (_massChanger != null) _massChanger.Setup(_storageController.MainStorage);
            }           
        }
        
        private void Update()
        {
            if (!IsVisible) return;
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
            if (!IsVisible) return;
            float t = Time.fixedDeltaTime;
            if (_truckConfig.UsesPhysics)
            {                
                _engine.Update(t);
            }
            if (_truckConfig.UseFuel)
            {
                _fuelModule.Update(t);
            }
            if (_truckConfig.UseIntegrity)
            {
                _integrityModule.Update(t);
            }
        }

        #region reposition
        public override void Teleport(VirtualPoint point, Action onTeleportComplete = null)
        {            
           // if (_trailerConnectorHandler.IsActivated) TrailerConnector.Teleport(initialPoint, point);
            _axisController.Teleport(point, onTeleportComplete);            
        }
        public override void Stabilize() => _axisController.Stabilize();
        public override void RecoveryAt(RecoveryPoint point)
        {         
            ClearCargo();
            Teleport(point.GetPoint(), null);
        }
        public override void PhysicsLock(Rigidbody point)
        {
            if (_physicsLock != null) Destroy(_physicsLock);
            _physicsLock = Rigidbody.gameObject.AddComponent<FixedJoint>();
            _physicsLock.connectedBody = point;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.ResetInertiaTensor();
        }
        public override void PhysicsUnlock(Rigidbody point = null)
        {
            if (_physicsLock != null && ( point == null || _physicsLock.connectedBody == point))
            {
                Destroy(_physicsLock);
                _physicsLock = null;
            }
        }
        #endregion

        #region controls
        override public void Move(Vector2 dir)
        {

        }
        override public void Gas() => _engine.Clutch(false);
        override public void Brake() => IsBraking = true;
        override public void ReleaseBrake() => IsBraking = false;
        override public void Reverse() => _engine.Clutch(true);
        override public void ReleaseGas() => _engine.ReleaseClutch();
        override public void Steer(float x)
        {
            _engine.SetSteer(x);
            //if (x == 0f) _axisController.Steer(0f);
        }

        #endregion

        public override IReadOnlyCollection<Vector3> GetVehicleBounds()
        {
            if (_trailerConnectorHandler.IsActivated)
            {
                var list = new List<Vector3>(base.GetVehicleBounds());
                TrailerConnector.GetTrailersBounds(ref list);
                return list;
            }
            else return base.GetVehicleBounds();
        }

        public override void ClearCargo(bool destroy = true)
        {
            Storage.MakeEmpty();
        }
        public override TradeContract FormCollectContract() =>  _collectModule.FormCollectContract();

        public void OnTrailerConnected(Trailer trailer)
        {
            if (_storageController is SingleVehicleStorage)
            {
                var host = _storageController.gameObject;
                Destroy(_storageController);
                var multipleStorage = host.AddComponent<MultipleVehicleStorage>();
                multipleStorage.Setup(new VisualStorageSettings[1] { new VisualStorageSettings(null, TruckConfig.StorageConfiguration) }); // #problem
                multipleStorage.AddStorage(trailer.GetStorage());
                _storageController = multipleStorage;
                UpdateStorageLink();
                OnStorageChangedEvent?.Invoke(_storageController.Storage);
            }
            else
            {
                if (_storageController == null) _storageController = gameObject.AddComponent<MultipleVehicleStorage>();
                (_storageController as MultipleVehicleStorage).AddStorage(trailer.GetStorage());
            }
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
            Rigidbody.ResetInertiaTensor();
        }
        public void OnTrailerDisconnected(Trailer trailer)
        {
            var storage = (_storageController as MultipleVehicleStorage);
            if (storage != null)
            {
                if (trailer.TryGetStorage(out var trailerStorage)) storage.RemoveStorage(trailerStorage);
            }
        }

        private void OnVehicleLoseIntegrity()
        {

        }

        public class Factory : PlaceholderFactory<UnityEngine.Object, Truck>
        {
        }
    }    
}
