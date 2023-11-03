using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class BaseRaycastSuspensionController : AxisControllerBase
	{
        [SerializeField] protected RaycastWheel[] _wheels;
        [SerializeField] protected WheelConfiguration _wheelSettings;
        [SerializeField] protected Rigidbody _rigidbody;
        [SerializeField] protected Transform _centerOfMass;
        protected int _castMask;
        protected float _startMass = 1f, _carSpeed = 0f, _maxSpeed = 10;

        public override Vector3 Forward => _rigidbody.transform.forward;
        public override Vector3 Position => _rigidbody.position;
        public override Quaternion Rotation => _rigidbody.rotation;

        private void Awake()
        {
            _castMask = GameConstants.GetCustomLayermask(CustomLayermask.Tyres);
            if (_centerOfMass != null) _rigidbody.centerOfMass = _centerOfMass.localPosition;
            _startMass = _rigidbody.mass;
        }

        virtual protected void FixedUpdate()
        {            
            float suspensionLength = _wheelSettings.SuspensionLength,
                maxOffset = _wheelSettings.MaxSuspensionOffset,
                maxSuspension = _wheelSettings.SuspensionLength + maxOffset,
                springStrength = _wheelSettings.SpringStrength, springDamper = _wheelSettings.SpringDamper   ;
            _carSpeed = Vector3.Dot(Forward, _rigidbody.velocity);

            foreach (var wheel in _wheels)
            {
                Vector3 pos = wheel.SuspensionPoint.position, up = wheel.SuspensionPoint.up;
                float scaleCf = wheel.ScaleCf;

                if (Physics.Raycast(pos, -up, maxDistance: maxSuspension * scaleCf, hitInfo: out var hit, layerMask: _castMask))
                {
                    float offset = (suspensionLength * scaleCf - hit.distance) / (maxOffset * scaleCf);
                    Vector3 tireVelocity = _rigidbody.GetPointVelocity(pos);
                    float suspensionVelocity = Vector3.Dot(up, tireVelocity);

                    Vector3 steeringForce = CalculateSteerForce(wheel, tireVelocity);
                    Vector3 accelForce = wheel.IsMotor ? CalculateAccelerationForce(wheel) : Vector3.zero;

                    float suspensionForce = (offset * springStrength) - (suspensionVelocity * springDamper);
                    _rigidbody.AddForceAtPosition(up * suspensionForce + accelForce + steeringForce, pos);

                    wheel.PositionWheel(hit.distance);
                }
                else
                {
                    wheel.PositionWheel(maxSuspension);
                    
                }
            }
        }
        private void Update()
        {
            foreach (var wheel in _wheels)
            {
                wheel.Spin(_carSpeed / wheel.WheelRadius * 20f * Time.deltaTime);
            }
        }

        protected virtual Vector3 CalculateSteerForce(RaycastWheel wheel, Vector3 tireVelocity)
        {
            Vector3 steeringDir = wheel.SuspensionPoint.right;
            float steeringVel = Vector3.Dot(steeringDir, tireVelocity);
            float desiredVelChange = -steeringVel * _wheelSettings.TireGripFactor;
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
            Vector3 steeringForce = _wheelSettings.TireMass * desiredAccel * steeringDir;
            return steeringForce;
        }
        protected virtual Vector3 CalculateAccelerationForce(RaycastWheel wheel) => Vector3.zero;

        public override void Stabilize()
        {

        }
        public override void Teleport(VirtualPoint point)
        {

        }
    }
}
