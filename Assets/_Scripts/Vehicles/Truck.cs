using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class Truck : Vehicle
    {
        [SerializeField] private TruckConfig _truckConfig;
        private float _steerValue = 0f, _moveValue = 0f;
        private Vector2 _moveDir = Vector2.up, _targetDir = Vector2.zero;
        public override Vector3 Position => transform.position;
        public override void Move(Vector2 targetDir) => _targetDir = targetDir;

        private void Start()
        {
            _moveDir = RealDir;
        }
        private void Update()
        {
            float t = Time.deltaTime;

            Vector3 moveDir3 = new Vector3(_moveDir.x, 0f, _moveDir.y).normalized;

            if (_targetDir.sqrMagnitude == 0f)
            {
                if (_moveValue != 0f) _moveValue = Mathf.MoveTowards(_moveValue, 0f, t * _truckConfig.NaturalDeceleration);
            }
            else
            {
                if (Vector2.Dot(_targetDir, _moveDir) > 0f)
                {
                    _moveDir = Vector3.RotateTowards(_moveDir, _targetDir, 90f * t, 0f);
                    _moveValue = Mathf.MoveTowards(_moveValue, 1f, t * _truckConfig.Acceleration);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDir3, Vector3.up), 90f * t * Mathf.Abs(_moveValue));
                }
                else
                {
                    _moveDir = Vector3.RotateTowards(_moveDir, -_targetDir, 90f * t, 0f);
                    //_moveValue = -1f;
                    _moveValue = Mathf.MoveTowards(_moveValue, -1f * _truckConfig.BackSpeedCf, t * _truckConfig.Deceleration);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(-moveDir3, Vector3.up), 90f * t * Mathf.Abs(_moveValue));
                }
                
                _targetDir = Vector2.zero;
            }
            if (_moveValue != 0f) transform.Translate(_moveValue * t * _truckConfig.MaxSpeed * moveDir3, Space.World);
        }
        private Vector2 RealDir {
            get
            {
                var fwd = transform.forward;
                return new Vector2(fwd.x, fwd.z).normalized;
            }
            }
    }
}
