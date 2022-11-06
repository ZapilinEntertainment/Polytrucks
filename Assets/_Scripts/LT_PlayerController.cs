using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// https://docs.unity3d.com/ru/2018.4/Manual/WheelColliderTutorial.html

namespace LoneTrucker
{
    public enum AcceleratorStatus : byte { Idle, Accelerate, Backway, BrakesDown }
    public sealed class LT_PlayerController : MonoBehaviour
    {
        [SerializeField] private bool _useAntiRoll = true;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private LT_TruckSettings _truckSettings;
      //  [SerializeField] private TruckSettings _truckSettings;
        public List<AxleInfo> axleInfos; // the information about each individual axle
        [SerializeField] private float _maxMotorTorque; // maximum torque the motor can apply to wheel
        public float maxSteeringAngle; // maximum steer angle the wheel can have
       // private GameManager _gameManager;
       // private CameraController _cameraController;
      //  private InputModule _inputModule;
        private AcceleratorStatus _acceleratorStatus;
        private float _steerProgress = 0f;
        private bool? _steerValue = null;
        private bool _isPaused = false, _steerValueRecalculation = false;
        public float Speed { get; private set; }
        public float MaxSpeed { get; private set; }
        public float AccelerationProgress { get; private set; }
        private const float MAX_HIT_SPEED = 40f;
        public const string TAG = "Player";


        #region controls
        public void ChangeAcceleratorStatus(AcceleratorStatus nas)
        {
            if (_acceleratorStatus == AcceleratorStatus.BrakesDown)
            {
                foreach (var a in axleInfos)
                {
                    if (a.motor)
                    {
                        a.leftWheel.brakeTorque = 0f;
                        a.rightWheel.brakeTorque = 0f;
                    }
                }
            }
            _acceleratorStatus = nas;

            if (_acceleratorStatus == AcceleratorStatus.Accelerate)
            {
               // _cameraController.ReturnCamera();
            }
            if (_acceleratorStatus != AcceleratorStatus.Accelerate)
            {
                foreach (var a in axleInfos)
                {
                    if (a.motor)
                    {
                        a.leftWheel.motorTorque = 0f;
                        a.rightWheel.motorTorque = 0f;
                    }
                }
                AccelerationProgress = 0f;
            }
          //  else _cameraController.ReturnCamera();
        }
        public void ChangeSteerValue(bool? x)
        {
            _steerValue = x;
            if (_steerValue == null)
            {
                _steerValueRecalculation = _steerProgress != 0f;                
            }
            else
            {
               // if (AccelerationProgress > 0f) _cameraController.ReturnCamera();
                if (_steerValue == true) _steerValueRecalculation = _steerProgress != 1f;
                else _steerValueRecalculation = _steerProgress != -1f;
            }
        }
        #endregion

        private void Update()
        {
            Speed = Mathf.Abs(_rigidbody.velocity.z);
            if (Speed < 0f) Speed *= -1f;
        }

        private void FixedUpdate()
        {
            if (_isPaused) return;

            float t = Time.fixedDeltaTime, motorTorque = 0f;
            bool motorTorqueChange = false, brakeTorqueChange = false;
            WheelCollider leftWheel, rightWheel;

            switch (_acceleratorStatus)
            {
                case AcceleratorStatus.Accelerate:
                    {
                        if (AccelerationProgress != 1f)
                        {
                            AccelerationProgress = Mathf.MoveTowards(AccelerationProgress, 1f, t / _truckSettings.FullAccelerationTime);
                            motorTorqueChange = true;
                            motorTorque = AccelerationProgress * _maxMotorTorque;
                        }
                        break;
                    }
                case AcceleratorStatus.Backway:
                    {
                        float x = _truckSettings.BackwaySpeedPercent * -1f;
                        if (AccelerationProgress != x)
                        {
                            AccelerationProgress = Mathf.MoveTowards(AccelerationProgress, x, t / _truckSettings.FullAccelerationTime);
                            motorTorqueChange = true;
                            motorTorque = AccelerationProgress * _maxMotorTorque;
                        }
                        break;
                    }
                case AcceleratorStatus.BrakesDown:
                    {
                        brakeTorqueChange = true;
                        break;
                    }
            }

            float travelL, travelR, antiRollForce;
            WheelHit hit;
            bool antirollToLeft, antirollToRight;

            if (_steerValueRecalculation)
            {
                float x = 0f;
                if (_steerValue != null)
                {
                    if (_steerValue == true) x = 1f;
                    else x = -1f;
                }
                if (_steerProgress == x) _steerValueRecalculation = false;
                _steerProgress = Mathf.MoveTowards(_steerProgress, x, t / _truckSettings.FullSteerTime);
            }
            float steerAngle = maxSteeringAngle * _steerProgress;

            foreach (AxleInfo axleInfo in axleInfos)
            {
                leftWheel = axleInfo.leftWheel;
                rightWheel = axleInfo.rightWheel;
                if (axleInfo.steering & _steerValueRecalculation)
                {
                    leftWheel.steerAngle = steerAngle;
                    rightWheel.steerAngle = steerAngle;
                }
                if (axleInfo.motor)
                {
                    if (motorTorqueChange)
                    {
                        leftWheel.motorTorque = motorTorque;
                        rightWheel.motorTorque = motorTorque;
                    }
                    if (brakeTorqueChange)
                    {
                        leftWheel.brakeTorque = _truckSettings.BrakingPower;
                        rightWheel.brakeTorque = _truckSettings.BrakingPower;
                    }
                }

                if (_useAntiRoll)
                {
                    // stabilizer bars https://forum.unity.com/threads/how-to-make-a-physically-real-stable-car-with-wheelcolliders.50643/
                    const float antirollVal = 35000f;
                    antirollToLeft = false;
                    antirollToRight = false;
                    travelL = travelR = 1f;
                    if (leftWheel.GetGroundHit(out hit))
                    {
                        travelL = (-leftWheel.transform.InverseTransformPoint(hit.point).y - leftWheel.radius) / leftWheel.suspensionDistance;
                    }
                    else
                    {
                        travelL = 1f;
                        antirollToLeft = true;
                    }
                    if (rightWheel.GetGroundHit(out hit))
                    {
                        travelR = (-rightWheel.transform.InverseTransformPoint(hit.point).y - rightWheel.radius) / rightWheel.suspensionDistance;
                    }
                    else
                    {
                        travelR = 1f;
                        antirollToRight = true;
                    }
                    antiRollForce = (travelL - travelR) * antirollVal;
                    if (antirollToRight == true) _rigidbody.AddForceAtPosition(leftWheel.transform.up * -antiRollForce, leftWheel.transform.position);
                    if (antirollToLeft == true) _rigidbody.AddForceAtPosition(rightWheel.transform.up * -antiRollForce, rightWheel.transform.position);
                }
            }
        }

        private void LateUpdate()
        {
            Vector3 position; Quaternion rotation; Transform t;
            foreach (AxleInfo axleInfo in axleInfos)
            {
                axleInfo.leftWheel.GetWorldPose(out position, out rotation);
                t = axleInfo._leftWheelModel;
                t.position = position;
                t.rotation = rotation;
                axleInfo.rightWheel.GetWorldPose(out position, out rotation);
                t = axleInfo._rightWheelModel;
                t.position = position;
                t.rotation = rotation;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var col = collision.collider;
            //if (col.CompareTag("Wall"))
            //{
            float hitVal = Mathf.Clamp01(Speed / MAX_HIT_SPEED);
            //_cameraController.ShakeCamera(hitVal);
            //Debug.Log(hitVal);
            //}
        }

        private void OnGUI()
        {
            GUILayout.Label(_rigidbody.velocity.ToString());
        }

        public void SetPause(bool x)
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public sealed class AxleInfo
    {
        public WheelCollider leftWheel, rightWheel;
        public Transform _leftWheelModel, _rightWheelModel;
        public bool motor; // is this wheel attached to motor?
        public bool steering; // does this wheel apply steer angle?
    }
}
