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
            _power = _axisController.MaxEngineSpeed * _startMass;
        }
        override protected void FixedUpdate()
        {
            _steerAngle = _axisController.SteerValue * _axisController.MaxSteerAngle;
            _steerRotation = Quaternion.Euler(0f, _steerAngle, 0f);
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
            Vector3 accelDir = wheel.IsSteer ? _steerRotation * wheel.SuspensionPoint.forward : wheel.SuspensionPoint.forward;
            float accelInput = _axisController.GasValue;
            if (accelInput != 0f)
            {
                float availableTorque = _axisController.CalculatePowerEffort(_speedPc) * accelInput;
                Vector3 accelForce = availableTorque * _power * accelDir;
                return accelForce;
            }
            else return Vector3.zero;
            
        }
    }
}
