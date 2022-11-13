using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public class TruckController : MonoBehaviour
    {
        [SerializeField] private TruckSettings _truckSettings;
        [SerializeField] private Storage _storage;
        private float _speed = 0f, _targetSpeed = 0f, _rotationAcceleration = 0f;
        private Vector3 _moveVector = Vector3.zero;

        public float SpeedPc => _speed / _maxspeed;
        private float _maxspeed => _truckSettings?.MaxSpeed ?? 1f;

        public Storage GetStorage() => _storage;
        public void Move(Vector2 dir)
        {
            _moveVector = new Vector3(dir.x, 0f, dir.y).normalized;
            _targetSpeed = dir.magnitude * _truckSettings.MaxSpeed;
        }

        private void Update()
        {
            float t = Time.deltaTime;
            if (_targetSpeed != 0f) _speed = Mathf.MoveTowards(_speed, _targetSpeed, t / _truckSettings.AccelerationTime *  _maxspeed);
            else _speed = Mathf.MoveTowards(_speed, 0f, t / _truckSettings.StopTime *  _maxspeed);

            if (_speed != 0f)
            {
                transform.Translate(Vector3.forward * _speed * t);
                _targetSpeed = Mathf.MoveTowards(_targetSpeed, 0f, t / _truckSettings.StopTime * _maxspeed);

                if (_moveVector.sqrMagnitude != 0f)
                {
                    Quaternion currentRotation = transform.rotation, targetRotation = Quaternion.LookRotation(_moveVector, transform.up);
                    if (currentRotation != targetRotation) _rotationAcceleration = Mathf.MoveTowards(_rotationAcceleration,  Mathf.Clamp01(Quaternion.Angle(currentRotation, targetRotation) / 180f), t);
                    else _rotationAcceleration = 0f;
                    transform.rotation = Quaternion.RotateTowards(currentRotation,targetRotation , _truckSettings.RotationSpeed * t * _rotationAcceleration);                   
                }
            }
        }

        public bool TryCollectionItem(in Item i) => _storage?.TryAddItem(i) ?? false;
    }
}
