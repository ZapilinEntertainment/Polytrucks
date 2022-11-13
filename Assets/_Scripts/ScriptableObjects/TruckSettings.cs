using UnityEngine;
namespace Polytrucks
{
    [CreateAssetMenu(fileName = "TruckSettings", menuName = "ScriptableObjects/TruckSettingsObject", order = 1)]
    public sealed class TruckSettings : ScriptableObject
    {
        [SerializeField] private float _maxSpeed = 10f, _accelerationTime = 1f, _stopTime = 0.25f, _rotationSpeed = 180f;

        public float MaxSpeed => _maxSpeed;
        public float AccelerationTime => _accelerationTime;
        public float StopTime => _stopTime;
        public float RotationSpeed => _rotationSpeed;
    }
}