using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class SimplePhysicsAxisController : AxisControllerBase
	{
        [SerializeField] private LocalPointAxle _fwdAxle;
        [SerializeField] private Rigidbody _rigidbody;
        public override Vector3 Forward => _fwdAxle?.Forward ?? transform.forward;
        public override Vector3 Position => _rigidbody.position;
        public override Quaternion Rotation => _rigidbody.rotation;

        private Vector3 applyPoint, forceVector;

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
            applyPoint = _fwdAxle.ForceApplyPosition;
            forceVector = step * _fwdAxle.Forward;
            _rigidbody.AddForce(step * _fwdAxle.Forward, ForceMode.VelocityChange);
        }
        public override void Steer(float value)
        {
            _fwdAxle.Steer(value);
        }
        public override void Teleport(VirtualPoint point)
        {
            _rigidbody.MovePosition(point.Position);
            _rigidbody.MoveRotation(point.Rotation);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(applyPoint, applyPoint + forceVector * 100f);
        }
    }
}
