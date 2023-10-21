using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SimplePhysicsAxisController : AxisControllerBase
	{
        [SerializeField] private LocalPointAxle _fwdAxle, _rearAxle;
        [SerializeField] private Rigidbody _rigidbody;
        private float _steer = 0f;
        public override Vector3 Forward => _fwdAxle?.Forward ?? transform.forward;
        public override Vector3 Position => _rigidbody.position;
        public override Quaternion Rotation => _rigidbody.rotation;

        protected override void OnSetup()
        {
            
        }
        public override void Stabilize()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.ResetInertiaTensor();
            _rigidbody.MovePosition(_rigidbody.position + Vector3.up * 4f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));
        }
        public override void Move(float step)
        {
            float steerCfRight = _fwdAxle.SteerCfRight, steerCfLeft = _fwdAxle.SteerCfLeft;
            _rigidbody.AddForceAtPosition(step * transform.forward * steerCfLeft, _fwdAxle.Forward, ForceMode.VelocityChange);            
            Debug.Log($"{steerCfLeft}-{steerCfRight}");
            _rigidbody.AddForceAtPosition(step * transform.forward * steerCfRight, _fwdAxle.Forward, ForceMode.VelocityChange);
            _rigidbody.AddForceAtPosition(step * _rearAxle.Forward, _rearAxle.RightWheelPos, ForceMode.VelocityChange);
            _rigidbody.AddForceAtPosition(step * _rearAxle.Forward, _rearAxle.LeftWheelPos, ForceMode.VelocityChange);

            _rigidbody.AddRelativeTorque(Vector3.up * _steer * step, ForceMode.VelocityChange);
        }
        public override void Steer(float value)
        {
            _steer = value;
            _fwdAxle.Steer(value);
        }
        public override void Teleport(VirtualPoint point)
        {
            _rigidbody.MovePosition(point.Position);
            _rigidbody.MoveRotation(point.Rotation);
        }

 
    }
}
