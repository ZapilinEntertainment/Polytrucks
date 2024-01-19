using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class SimpleAxisController : AxisControllerBase
	{
		[SerializeField] private VirtualPointAxle _fwdAxle, _rearAxle;
        private float _axisDistance = 1f, _centerDistance;
        public override float Speed => throw new System.NotImplementedException();
        public override Vector3 Forward => _fwdAxle?.Forward ?? transform.forward;
        public override Vector3 Position => transform.position;
        public override Quaternion Rotation => transform.rotation;

        protected override void OnSetup()
        {
            _fwdAxle.Setup(this);
            _rearAxle.Setup(this);   
            _axisDistance = (_fwdAxle.Position - _rearAxle.Position).magnitude;
            _centerDistance = (_fwdAxle.Position - Position).magnitude;
        }
        public override void Stabilize()
        {
            throw new System.NotImplementedException();
        }
        private void Update()
        {
            if (IsActive)
            {
                _fwdAxle.Steer(Truck.SteerValue * TruckConfig.MaxSteerAngle);

                VirtualPoint fwdPoint = _fwdAxle.Move(Time.deltaTime * Truck.GasValue * TruckConfig.MaxSpeed);

                Vector3 dir = (fwdPoint.Position - _rearAxle.Position).normalized;
                _rearAxle.Move(fwdPoint.Position - dir * _axisDistance);
                SetPoint(fwdPoint.Position - dir * _centerDistance, Quaternion.LookRotation(dir, Vector3.up));
            }
        }
        public override void Teleport(VirtualPoint point)
        {
            SetPoint(point.Position, point.Rotation);
            _fwdAxle.SyncToTransform();
            _rearAxle.SyncToTransform();
        }
        private void SetPoint(Vector3 pos, Quaternion rotation) => transform.SetPositionAndRotation(pos, rotation);
    }
}
