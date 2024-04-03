using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(fileName = "TruckConfig", menuName = "ScriptableObjects/TruckConfig", order = 1)]
    public sealed class TruckConfig : ScriptableObject
	{
        [SerializeField]
        private float _maxSpeed = 15f, _maxSteerAngle = 60f, _steerTime = 0.5f,
            _acceleration = 5f, _reverseAcceleration = 4f, _engineStopTime = 1f,
            _reverseSpeedCf = 0.5f, _mass = 100f, _passability = 1f;
        [SerializeField] private AnimationCurve _rotationToSpeedCurve, _steerCurve, _powerCurve;
		[SerializeField] private StorageConfiguration _storageConfiguration;
		[SerializeField] private FuelConfiguration _fuelConfiguration;
		[SerializeField] private IntegrityConfiguration _integrityConfiguration;

        [field: SerializeField] public TruckID TruckID { get; private set; } = TruckID.Undefined;
		[field: SerializeField] public TrailerID TrailerID { get; private set; } = TrailerID.NoTrailer;
        [field: SerializeField] public bool UsesPhysics { get; private set; } = false;		
		[field: SerializeField] public float CollectTime { get; private set; } = 0.25f;
		[Tooltip("Test-defined value, displayed in garage")] [field: SerializeField] public float AccelerationMeterResult { get; private set; } = 10f;

		
		public bool HasCargoSpace => _storageConfiguration != null;		
		public bool UseFuel => _fuelConfiguration != null;
		public bool UseIntegrity => _integrityConfiguration!= null;
		public bool TrailerRequired => !HasCargoSpace && TrailerID != TrailerID.NoTrailer;
		public float MaxSpeed => _maxSpeed;
		public float MaxSteerAngle => _maxSteerAngle;
		public float CalculateSpeedCf(float steerValue) => _rotationToSpeedCurve.Evaluate(steerValue);
		public float CalculateSteer(float steerValue) => _steerCurve.Evaluate(steerValue);
		public float CalculatePowerEffort(float speedPc) => _powerCurve.Evaluate(speedPc);
		public StorageConfiguration StorageConfiguration => _storageConfiguration;
		public FuelConfiguration FuelConfiguration => _fuelConfiguration;
		public IntegrityConfiguration IntegrityConfiguration => _integrityConfiguration;

		public float Acceleration => _acceleration;
		public float ReverseAcceleration => _reverseAcceleration;
		public float ReverseSpeedCf => _reverseSpeedCf;
		public float SteerTime => _steerTime;
		public float Mass => _mass;
		public float Passability => _passability;

		public float GetParameterValue(TruckParameterType parameter)
		{
			switch (parameter)
			{
				case TruckParameterType.MaxSpeed: return _maxSpeed;
				case TruckParameterType.Acceleration: return AccelerationMeterResult;
				case TruckParameterType.Mass: return Mass;
				case TruckParameterType.Passability: return Passability;
				case TruckParameterType.Capacity: return _storageConfiguration?.Capacity ?? 0;
				default: return 0f;
			}
		}
	}
}
