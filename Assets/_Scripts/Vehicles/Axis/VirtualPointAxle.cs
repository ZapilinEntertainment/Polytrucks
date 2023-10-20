using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

    
	public class VirtualPointAxle : AxleBase
	{
        // axle for transform-movement objects

		[SerializeField] private bool _isSteer = false, _isMotor = false;
        
        private VirtualPoint _virtualPoint;
        override public Vector3 Position => _virtualPoint.Position;
        override public Vector3 Forward => _virtualPoint.Forward;

        override public void Setup(AxisControllerBase axisController)
        {
            if (_leftWheel == null || _rightWheel == null)
            {
                Debug.LogError("axle wheels not set");
                return;
            }
            SyncToTransform();
        }
        public void SyncToTransform()
        {
            _virtualPoint = new VirtualPoint(_leftWheel, _rightWheel);
        }

        public VirtualPoint Move(float step)
        {
            _virtualPoint = _virtualPoint.Move(step);
            return _virtualPoint;
        }        
        public VirtualPoint Move(Vector3 pos)
        {
            _virtualPoint = new VirtualPoint()
            {
                Position = pos,
                Rotation = _virtualPoint.Rotation
            };
            return _virtualPoint;
        }
        override public void Steer(float steerAngle)
        {
            _virtualPoint = _virtualPoint.Steer(transform.rotation * Quaternion.AngleAxis(steerAngle, _virtualPoint.Up));

            Quaternion wheelRotation = Quaternion.Euler(0f,steerAngle,0f);
           _leftWheel.localRotation = wheelRotation;
            _rightWheel.localRotation = wheelRotation;
        }
    }
}
