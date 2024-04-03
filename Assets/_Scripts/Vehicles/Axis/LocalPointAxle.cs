using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class LocalPointAxle : AxleBase
	{
        private float _steerAngle;
        public float SteerCfLeft => Mathf.Clamp01( 1f + _steerAngle / 90f);
        public float SteerCfRight => Mathf.Clamp01( 1f - _steerAngle / 90f);
        public override Vector3 Position => transform.position;
        public override Vector3 Forward => Quaternion.AngleAxis(_steerAngle, transform.up) * transform.forward;
        public Vector3 LeftWheelPos => _leftWheel.position;
        public Vector3 RightWheelPos => _rightWheel.position;

        public override void Setup(AxisControllerBase axisController)
        {
            _steerAngle = 0f;
        }
        override public void Steer(float steerAngle)
        {
            _steerAngle = steerAngle;

            Quaternion wheelRotation = Quaternion.Euler(0f, steerAngle, 0f);
            _leftWheel.localRotation = wheelRotation;
            _rightWheel.localRotation = wheelRotation;
        }
    }
}
