using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SimplePhysicsAxisController : AxisControllerBase
	{
        [SerializeField] private LocalPointAxle _fwdAxle, _rearAxle;
        [SerializeField] private Rigidbody _rigidbody;
        public override float Speed => throw new System.NotImplementedException();
        public override Vector3 Forward => _fwdAxle?.Forward ?? transform.forward;
        public override Vector3 Position => _rigidbody.position;
        public override Quaternion Rotation => _rigidbody.rotation;

        public override void Stabilize()
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
            _rigidbody.ResetInertiaTensor();
            _rigidbody.MovePosition(_rigidbody.position + Vector3.up * 4f);
            _rigidbody.MoveRotation(Quaternion.LookRotation(transform.forward, Vector3.up));
        }
        private void FixedUpdate()
        {
            if (IsActive)
            {
                float steer = _axisController.SteerValue;
                _fwdAxle.Steer(steer * _axisController.MaxSteerAngle);

                float steerCfRight = _fwdAxle.SteerCfRight, steerCfLeft = _fwdAxle.SteerCfLeft;
                float step = Time.fixedDeltaTime * _axisController.GasValue * _axisController.MaxEngineSpeed;
                _rigidbody.AddForceAtPosition(step * transform.forward * steerCfLeft, _fwdAxle.Forward, ForceMode.VelocityChange);
                //Debug.Log($"{steerCfLeft}-{steerCfRight}");
                _rigidbody.AddForceAtPosition(step * transform.forward * steerCfRight, _fwdAxle.Forward, ForceMode.VelocityChange);
                _rigidbody.AddForceAtPosition(step * _rearAxle.Forward, _rearAxle.RightWheelPos, ForceMode.VelocityChange);
                _rigidbody.AddForceAtPosition(step * _rearAxle.Forward, _rearAxle.LeftWheelPos, ForceMode.VelocityChange);

                _rigidbody.AddRelativeTorque(Vector3.up * steer * step, ForceMode.VelocityChange);
            }
        }
        public override void Teleport(VirtualPoint point)
        {
            _rigidbody.MovePosition(point.Position);
            _rigidbody.MoveRotation(point.Rotation);
        }

 
    }
}
