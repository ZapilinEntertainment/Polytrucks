using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public class RaycastWheelsAxle : BaseRaycastSuspensionController
	{
        // tutorial link: https://www.youtube.com/watch?v=CdPYlj5uZeI
        protected float _steerAngle = 0f, _power;
        protected Quaternion _steerRotation;

        protected override void OnSetup()
        {
            if (AxisController == null) return;
            _power = AxisController.MaxEngineSpeed * _startMass;
        }
        override protected void FixedUpdate()
        {
            if (IsActive)
            {
                _steerAngle = AxisController.SteerValue * AxisController.MaxSteerAngle;
                _steerRotation = Quaternion.Euler(0f, _steerAngle, 0f);
            }
            base.FixedUpdate();
        }

        protected override Vector3 CalculateSteerForce(RaycastWheel wheel, Vector3 tireVelocity)
        {
            bool isSteer = wheel.IsSteer;
           
            Vector3 steeringDir = isSteer ? _steerRotation * wheel.SuspensionPoint.right : wheel.SuspensionPoint.right;
            float steeringVel = Vector3.Dot(steeringDir, tireVelocity);
            float desiredVelChange = -steeringVel * _wheelSettings.TireGripCurve.Evaluate(_speedPc);
            float desiredAccel = desiredVelChange / Time.fixedDeltaTime;
            Vector3 steeringForce = _wheelSettings.TireMass * desiredAccel * steeringDir;
            if (isSteer) wheel.SetSteer(_steerAngle);
            return steeringForce;
        }
        protected override Vector3 CalculateAccelerationForce(RaycastWheel wheel)
        {
            float accelInput = AxisController.GasValue;
            if (accelInput != 0f)
            {
                Vector3 accelDir = wheel.IsSteer ? _steerRotation * wheel.SuspensionPoint.forward : wheel.SuspensionPoint.forward;
                float availableTorque = AxisController.CalculatePowerEffort(_speedPc) * accelInput;
                Vector3 accelForce = availableTorque * _power * accelDir;
                return accelForce;
            }
            else return Vector3.zero;
        }
    }
}
