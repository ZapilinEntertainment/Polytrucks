using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {

    public struct VirtualPoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Forward => Rotation * Vector3.forward;
        public Vector3 Up => Rotation * Vector3.up;

        public VirtualPoint(Transform leftWheel, Transform rightWheel)
        {
            Position = Vector3.Lerp(leftWheel.position, rightWheel.position, 0.5f);
            Position.y = GameConstants.GROUND_HEIGHT;
            Rotation = Quaternion.Lerp(leftWheel.rotation, rightWheel.rotation, 0.5f);
        }
        public VirtualPoint Move( float step)
        {
            return new VirtualPoint()
            {
                Position = Position + step * Forward,
                Rotation = Rotation
            };
        }
        public VirtualPoint Steer(Quaternion rotation)
        {
            return new VirtualPoint()
            {
                Position = Position,
                Rotation = rotation
            };
        }
    }
	public class Axle : MonoBehaviour
	{
		[SerializeField] private bool _isSteer = false, _isMotor = false;
        [SerializeField] private Transform _leftWheel, _rightWheel;
        private VirtualPoint _virtualPoint;
        public Vector3 Position => _virtualPoint.Position;
        public Vector3 Forward => _virtualPoint.Forward;

        public void Setup(AxisControllerBase axisController)
        {
            if (_leftWheel == null || _rightWheel == null)
            {
                Debug.LogError("axle wheels not set");
                return;
            }
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
        public void Steer(float steerAngle)
        {
            _virtualPoint = _virtualPoint.Steer(transform.rotation * Quaternion.AngleAxis(steerAngle, _virtualPoint.Up));

            Quaternion wheelRotation = Quaternion.Euler(0f,steerAngle,0f);
           _leftWheel.localRotation = wheelRotation;
            _rightWheel.localRotation = wheelRotation;
        }
    }
}
