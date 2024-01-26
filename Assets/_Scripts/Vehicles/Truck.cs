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
       
        public override Vector3 Position => _axisController.Position;
        public override VirtualPoint FormVirtualPoint() => new VirtualPoint() { Position = _axisController.Position, Rotation = _axisController.Rotation };
        public float CalculatePowerEffort(float pc) => _truckConfig.CalculatePowerEffort(pc);

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
            _engine = new TruckEngine(_truckConfig, _axisController);
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
        public override TradeContract FormCollectContract() => _collectModule.FormCollectContract();
        public override bool CanFulfillContract(TradeContract contract) => Storage.CanFulfillContract(contract);
        public override int LoadCargo(VirtualCollectable item, int count) => Storage.AddItems(item, count);
        public override bool TryLoadCargo(VirtualCollectable item, int count) => Storage.TryLoadCargo(item, count);

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

        public class Factory : PlaceholderFactory<UnityEngine.Object, Truck>
        {
        }
    }    
}
