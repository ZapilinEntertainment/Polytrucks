using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class TruckEngine
    {
        private bool _isClutched = false, _isReversed = false;
        private float _steerTarget = 0f, _gasValue = 0f;
        private TruckConfig _config;
        private AxisControllerBase _axis;
        virtual protected bool CanAccelerate => _isClutched;
        public float GasValue
        {
            get
            {
                if (_isClutched)
                {
                    if (_isReversed) return _gasValue * _config.ReverseSpeedCf * -1f;
                    else return _gasValue;
                }
                else return 0f;
            }
        }
        public float SteerValue { get; private set; } = 0f;
        public TruckEngine(TruckConfig config, AxisControllerBase axis)
        {
            _config = config;
            _axis = axis;
        }

        public void Update(float deltaTime)
        {

            float speedCf;
            if (_steerTarget == 0f) speedCf = 10f;
            else speedCf = 1f;
            if (_steerTarget != SteerValue) SteerValue = Mathf.MoveTowards(SteerValue, _steerTarget, (deltaTime / _config.SteerTime) * speedCf) ;

            float gasTarget;
            float deltaSpeed;

            if (CanAccelerate)
            {
                gasTarget = _config.CalculateSpeedCf(SteerValue);
                deltaSpeed = _config.Acceleration * deltaTime;
            }
            else
            {
                gasTarget = 0f;
                deltaSpeed = _config.ReverseAcceleration* deltaTime;
            }
            _gasValue = Mathf.MoveTowards(_gasValue, gasTarget, deltaSpeed);
        }

        public void Clutch(bool reverse)
        {
            _isClutched = true;
            _isReversed= reverse;
        }
        public void ReleaseClutch()
        {
            _isClutched = false;
        }
        public void SetSteer(float x)
        {
            _steerTarget = x;
            if (_steerTarget * SteerValue != 0f && SteerValue / _steerTarget < 0f) SteerValue = 0f;
        }
    }
}
