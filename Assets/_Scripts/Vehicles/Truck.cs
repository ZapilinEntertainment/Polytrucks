using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class Truck : Vehicle,ICollector, ISeller
    {
        #region gas states
        private abstract class MoveState
        {
            protected TruckEngine _engine;
            public MoveState(TruckEngine engine)
            {
                _engine = engine;
            }
            abstract public void Update(float deltaTime);
        }
        private class FreeState:MoveState
        {
            public FreeState(TruckEngine engine) : base(engine) { }
            public override void Update(float deltaTime)
            {
                _engine.GasValue = Mathf.MoveTowards(_engine.GasValue, 0f, _engine.NaturalDeceleration * deltaTime);
            }
        }
        private class AccelerationState : MoveState
        {
            public AccelerationState(TruckEngine engine) : base(engine) { }
            public override void Update(float deltaTime)
            {
                _engine.GasValue = Mathf.MoveTowards(_engine.GasValue, 1f, _engine.Acceleration * deltaTime);
            }
        }
        private class BrakeState : MoveState
        {
            public BrakeState(TruckEngine engine) : base(engine) { }
            public override void Update(float deltaTime)
            {
                _engine.GasValue = Mathf.MoveTowards(_engine.GasValue, 0f, _engine.BrakeSpeed * deltaTime);
            }
        }
        private class ReverseState : BrakeState
        {
            public ReverseState(TruckEngine engine) : base(engine) { }
            public override void Update(float deltaTime)
            {
                if (_engine.GasValue > 0f) base.Update(deltaTime);
                else
                {
                    _engine.GasValue = Mathf.MoveTowards(_engine.GasValue, -1f, deltaTime * _engine.ReverseAcceleration);
                }
            }
        }
        #endregion
        private class TruckEngine
        {
            private MoveState _currentMoveState, _accelerationState, _freeState, _reverseState, _brakeState;
            private TruckConfig _config;
            private AxisControllerBase _axis;
            public float GasValue = 0f, SteerValue = 0f;
            public float Acceleration => _config.Acceleration;
            public float NaturalDeceleration => _config.NaturalDeceleration;
            public float ReverseAcceleration => _config.Acceleration * _config.ReverseSpeedCf;
            public float BrakeSpeed => _config.BrakeDeceleration;

            public TruckEngine(TruckConfig config, AxisControllerBase axis) {
                _config = config;
                _axis = axis;
                _freeState = new FreeState(this);
                _accelerationState = new AccelerationState(this);
                _brakeState = new BrakeState(this);
                _reverseState = new ReverseState(this);

                _currentMoveState = _freeState;
            }            

            public void Update(float deltaTime)
            {
                _currentMoveState.Update(deltaTime);
                _axis.Steer(SteerValue * _config.MaxSteerAngle);

                float speed;
                if (GasValue > 0f) speed = _config.MaxSpeed * GasValue * deltaTime;
                else speed = _config.MaxSpeed * GasValue * _config.ReverseSpeedCf * deltaTime;
                speed *= _config.CalculateSpeedCf(SteerValue);
                _axis.Move(speed);                
            }
            public void Gas()
            {
                _currentMoveState = _accelerationState;
            }
            public void Brake()
            {
                _currentMoveState = _brakeState;
            }
            public void ReleaseControl()
            {
                _currentMoveState = _freeState;
            }
            public void Reverse()
            {
                _currentMoveState = _reverseState;
            }
        }

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
            //if (GameSessionActive)
            {
                float t = Time.deltaTime;
                _engine.Update(t);
            }
        }

        public override void Teleport(VirtualPoint point) => _axisController.Teleport(point);

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
            _engine.SteerValue = x;
            _axisController.Steer(x);
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
