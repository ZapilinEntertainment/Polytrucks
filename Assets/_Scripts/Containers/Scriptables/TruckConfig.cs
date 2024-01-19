using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "TruckConfig", menuName = "ScriptableObjects/TruckConfig", order = 1)]
    public sealed class TruckConfig : ScriptableObject
	{
        [field: SerializeField] public TruckID TruckID { get; private set; } = TruckID.Undefined;
        [field: SerializeField] public bool UsesPhysics { get; private set; } = false;
		
		[SerializeField] private float _maxSpeed = 15f, _maxSteerAngle = 60f, _steerTime = 0.5f,
			_acceleration = 5f, _naturalDeceleration = 1f, _brakeDeceleration = 4f, _reverseSpeedCf = 0.5f, _mass = 100f, _passability = 1f;
		[SerializeField] private AnimationCurve _rotationToSpeedCurve, _steerCurve, _powerCurve;
		[field: SerializeField] public float CollectTime { get; private set; } = 0.25f;

		public float MaxSpeed => _maxSpeed;
		public float MaxSteerAngle => _maxSteerAngle;
		public float CalculateSpeedCf(float steerValue) => _rotationToSpeedCurve.Evaluate(steerValue);
		public float CalculateSteer(float steerValue) => _steerCurve.Evaluate(steerValue);
		public float CalculatePowerEffort(float speedPc) => _powerCurve.Evaluate(speedPc);

		public float Acceleration => _acceleration;
		public float NaturalDeceleration => _naturalDeceleration;
		public float BrakeDeceleration => _brakeDeceleration;
		public float ReverseSpeedCf => _reverseSpeedCf;
		public float SteerTime => _steerTime;
		public float Mass => _mass;
		public float Passability => _passability;
	}
}
