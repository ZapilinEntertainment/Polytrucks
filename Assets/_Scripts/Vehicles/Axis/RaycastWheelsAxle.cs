using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class RaycastWheelsAxle : AxisControllerBase
	{
		[Serializable]
		private struct RaycastWheel
		{
            public Transform WheelModel;
            public Transform SuspensionPoint;
            public bool IsMotor, IsSteer;
            public float WheelRadius;

            public void PositionWheel(float y) => WheelModel.position = SuspensionPoint.TransformPoint(Vector3.down * (y - WheelRadius));
        }
		[Serializable]
		private class WheelSettings
		{
			public float SuspensionLength = 1f;
			public float SpringStrength = 100f;
			public float SpringDamper = 10f;
            public float MaxSuspensionOffset = 1f;
		}

		[SerializeField] private RaycastWheel[] _wheels;
		[SerializeField] private WheelSettings _wheelSettings;
		[SerializeField] private Rigidbody _rigidbody;
		[SerializeField] private Transform _centerOfMass;
		private int _castMask;

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
                springStrength = _wheelSettings.SpringStrength, springDamper = _wheelSettings.SpringDamper;
            foreach (var wheel in _wheels)
			{
                Vector3 pos = wheel.SuspensionPoint.position, up = wheel.SuspensionPoint.up; 
                
                if (Physics.Raycast(pos, -up, maxDistance: maxSuspension, hitInfo: out var hit, layerMask: _castMask))
                {
                    float offset = (suspensionLength - hit.distance) / maxOffset;
                    Vector3 velocity = _rigidbody.GetPointVelocity(pos);
                    float vel = Vector3.Dot(up, velocity);

                    float force = (offset * springStrength) - (vel * springDamper);
                    _rigidbody.AddForceAtPosition(up * force, pos);

                    wheel.PositionWheel(hit.distance);
                }
                else
                {
                    wheel.PositionWheel(maxSuspension);
                }
            }
        }
       

        protected override void OnSetup()
        {
            
        }

        public override void Stabilize()
        {
            
        }

        public override void Move(float step)
        {
           
        }

        public override void Steer(float angle)
        {
           
        }

        public override void Teleport(VirtualPoint point)
        {
            
        }
    }
}
