using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class RaycastWheelsAxle : AxisControllerBase
	{
        // tutorial link: https://www.youtube.com/watch?v=CdPYlj5uZeI

        [Serializable]
		private struct RaycastWheel
		{
            public Transform WheelModel;
            public Transform SuspensionPoint;
            public bool IsMotor, IsSteer;
            public float WheelRadius;

            public void PositionWheel(float y) => WheelModel.position = SuspensionPoint.TransformPoint(Vector3.down * (y - WheelRadius));
            public void SetSteer(float x) => WheelModel.localRotation = Quaternion.Euler(0f, x, 0f);
        }
		[Serializable]
		private class WheelSettings
		{
			public float SuspensionLength = 1f;
			public float SpringStrength = 100f;
			public float SpringDamper = 10f;
            public float MaxSuspensionOffset = 1f;
            public float TireGripFactor = 1f;
            public float TireMass = 1f;
            public float Power = 50f;
            public AnimationCurve PowerCurve;
		}

		[SerializeField] private RaycastWheel[] _wheels;
		[SerializeField] private WheelSettings _wheelSettings;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Transform _centerOfMass;
		private int _castMask;
        private float _steerAngle = 0f;
        private Vehicle _vehicle;

		public override Vector3 Forward => _rigidbody.transform.forward;

		public override Vector3 Position => _rigidbody.position;

		public override Quaternion Rotation => _rigidbody.rotation;

        private void Awake()
        {
			_castMask = GameConstants.GetCustomLayermask(CustomLayermask.Tyres);
			_rigidbody.centerOfMass = _centerOfMass.localPosition;
        }
        private void FixedUpdate()
        {
			float t = Time.fixedDeltaTime;

            float suspensionLength = _wheelSettings.SuspensionLength, 
                maxOffset = _wheelSettings.MaxSuspensionOffset,
                maxSuspension = _wheelSettings.SuspensionLength + maxOffset,
                springStrength = _wheelSettings.SpringStrength, springDamper = _wheelSettings.SpringDamper,
                tireGrip = _wheelSettings.TireGripFactor,
                wheelMass = _wheelSettings.TireMass;


            Quaternion steerRotation = Quaternion.Euler(0f, _steerAngle, 0f);
            foreach (var wheel in _wheels)
			{
                Vector3 pos = wheel.SuspensionPoint.position, up = wheel.SuspensionPoint.up; 
                
                if (Physics.Raycast(pos, -up, maxDistance: maxSuspension, hitInfo: out var hit, layerMask: _castMask))
                {
                    float offset = (suspensionLength - hit.distance) / maxOffset;
                    Vector3 tireVelocity = _rigidbody.GetPointVelocity(pos);
                    float vel = Vector3.Dot(up, tireVelocity);

                    bool isSteer = wheel.IsSteer;
                    Vector3 steeringDir = isSteer ? steerRotation * wheel.SuspensionPoint.right : wheel.SuspensionPoint.right;
                    float steeringVel = Vector3.Dot(steeringDir, tireVelocity);
                    float desiredVelChange = -steeringVel * tireGrip;
                    float desiredAccel = desiredVelChange / t;
                    Vector3 steeringForce = wheelMass * desiredAccel * steeringDir;
                    if (isSteer) wheel.SetSteer(_steerAngle);

                    Vector3 accelForce = Vector3.zero;
                    if (wheel.IsMotor)
                    {
                        Vector3 accelDir = wheel.IsSteer ? steerRotation * wheel.SuspensionPoint.forward : wheel.SuspensionPoint.forward;
                        float accelInput = _vehicle.GasValue;
                        if (accelInput != 0f)
                        {
                            float carSpeed = Vector3.Dot(Forward, _rigidbody.velocity);
                            float normalizedSpeed = Mathf.Clamp01(carSpeed / 30f);
                            float availableTorque = _wheelSettings.PowerCurve.Evaluate(normalizedSpeed) * accelInput;
                            accelForce = availableTorque * _wheelSettings.Power * accelDir;
                        }
                    }
                    

                    float force = (offset * springStrength) - (vel * springDamper);
                    _rigidbody.AddForceAtPosition(up * force + accelForce + steeringForce, pos);

                    wheel.PositionWheel(hit.distance);
                }
                else
                {
                    wheel.PositionWheel(maxSuspension);
                }
            }
        }
       

        public override void Setup(Vehicle vehicle)
        {
            _vehicle = vehicle;
        }

        public override void Stabilize()
        {
            
        }

        public override void Move(float step)
        {
           
        }

        public override void Steer(float angleDegree)
        {
            _steerAngle = angleDegree;
        }

        public override void Teleport(VirtualPoint point)
        {
            
        }
    }
}
