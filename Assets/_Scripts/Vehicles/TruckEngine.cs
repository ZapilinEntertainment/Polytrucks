using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class TruckEngine
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
        private class FreeState : MoveState
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

        private float _steerTarget = 0f;
        private MoveState _currentMoveState, _accelerationState, _freeState, _reverseState, _brakeState;
        private TruckConfig _config;
        private AxisControllerBase _axis;
        public float GasValue = 0f;
        public float SteerValue { get; private set; }
        public float Acceleration => _config.Acceleration;
        public float NaturalDeceleration => _config.NaturalDeceleration;
        public float ReverseAcceleration => _config.Acceleration * _config.ReverseSpeedCf;
        public float BrakeSpeed => _config.BrakeDeceleration;

        public TruckEngine(TruckConfig config, AxisControllerBase axis)
        {
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

            float speedCf;
            if (_steerTarget == 0f) speedCf = 10f;
            else speedCf = 1f;
            if (_steerTarget != SteerValue) SteerValue = Mathf.MoveTowards(SteerValue, _steerTarget, (deltaTime / _config.SteerTime) * speedCf) ;

            float speed;
            if (GasValue > 0f) speed = _config.MaxSpeed * GasValue * deltaTime;
            else speed = _config.MaxSpeed * GasValue * _config.ReverseSpeedCf * deltaTime;
            speed *= _config.CalculateSpeedCf(SteerValue);
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
        public void SetSteer(float x)
        {
            _steerTarget = x;
            if (_steerTarget * SteerValue != 0f && SteerValue / _steerTarget < 0f) SteerValue = 0f;
        }
    }
}
