using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class SimpleAxisController : AxisControllerBase
	{
		[SerializeField] private Axle _fwdAxle, _rearAxle;
        private float _axisDistance = 1f, _centerDistance;
        public override Vector3 Forward => _fwdAxle?.Forward ?? transform.forward;

        protected override void OnSetup()
        {
            _fwdAxle.Setup(this);
            _rearAxle.Setup(this);   
            _axisDistance = (_fwdAxle.Position - _rearAxle.Position).magnitude;
            _centerDistance = (_fwdAxle.Position - transform.position).magnitude;
        }
        public override void Move(float step)
        {
            VirtualPoint fwdPoint = _fwdAxle.Move(step);

            Vector3 dir = (fwdPoint.Position - _rearAxle.Position).normalized;
            _rearAxle.Move(fwdPoint.Position - dir * _axisDistance);
            transform.position = fwdPoint.Position - dir * _centerDistance;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
        public override void Steer(float value)
        {
            _fwdAxle.Steer(value);
        }
    }
}
