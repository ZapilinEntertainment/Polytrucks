using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "TruckConfig", menuName = "ScriptableObjects/TruckConfig", order = 1)]
    public sealed class TruckConfig : ScriptableObject
	{
		[SerializeField] private float _maxSpeed = 15f, _maxSteerAngle = 60f, 
			_acceleration = 5f, _naturalDeceleration = 1f, _brakeDeceleration = 4f, _reverseSpeedCf = 0.5f;
		[SerializeField] private AnimationCurve _rotationToSpeedCurve;

		public float MaxSpeed => _maxSpeed;
		public float MaxSteerAngle => _maxSteerAngle;
		public float CalculateSpeedCf(float steerValue) => _rotationToSpeedCurve.Evaluate(steerValue);

		public float Acceleration => _acceleration;
		public float NaturalDeceleration => _naturalDeceleration;
		public float BrakeDeceleration => _brakeDeceleration;
		public float ReverseSpeedCf => _reverseSpeedCf;
	}
}
