using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class Truck : Vehicle
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
        private TruckEngine _engine;

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
        public TruckConfig TruckConfig => _truckConfig;
        public override Vector3 Position => transform.position;

        private void Start()
        {
            _moveDir = RealDir;
            _engine = new TruckEngine(_truckConfig, _axisController);
            _axisController.Setup();
        }
        override public void Move(Vector2 dir)
        {

        }
        private void Update()
        {
            //if (GameSessionActive)
            {
                float t = Time.deltaTime;
                _engine.Update(t);
            }
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
    }
}