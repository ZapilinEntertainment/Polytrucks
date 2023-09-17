using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "TruckConfig", menuName = "ScriptableObjects/TruckConfig", order = 1)]
    public sealed class TruckConfig : ScriptableObject
	{
		[SerializeField] private float _maxSpeed = 15f, _acceleration = 3f, _deceleration = 2f, _naturalDeceleration = 1f,
			_steerSetTime = 0.5f, 
			_steerReturnTime = 0.1f, _backSpeedCf = 0.5f;

		public float MaxSpeed => _maxSpeed;
		public float Acceleration => _acceleration;
		public float Deceleration => _deceleration;
		public float NaturalDeceleration => _naturalDeceleration;
		public float SteeringSetTime => _steerSetTime;
		public float SteerReturnTime => _steerReturnTime;
		public float BackSpeedCf => _backSpeedCf;	
	}
}
