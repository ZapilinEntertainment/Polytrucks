using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class TruckModelController : MonoBehaviour
	{
		[SerializeField] private float _maxXAngle = 5f, _maxZAngle = 5f, _xSpeed = 1f, _zSpeed =1f;
		[SerializeField] private Transform _model;
		[SerializeField] private Truck _truck;
        private float _xValue, _zValue;

        private void LateUpdate()
        {
            float t = Time.deltaTime;
            _xValue = Mathf.MoveTowards(_xValue, _truck.GasValue, t * _xSpeed);
            _zValue = Mathf.MoveTowards(_zValue, _truck.SteerValue * _truck.SteerValue * Mathf.Sign(_truck.SteerValue) * _truck.GasValue, t * _zSpeed);
            _model.transform.localRotation = Quaternion.Euler(_xValue * _maxXAngle, 0f, _zValue * _maxZAngle);
        }
    }
}
