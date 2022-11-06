using UnityEngine;
namespace Polytrucks
{
    [CreateAssetMenu(fileName = "TruckSettings", menuName = "ScriptableObjects/TruckSettingsObject", order = 1)]
    public sealed class TruckSettings : ScriptableObject
    {
        [SerializeField] private float _maxSpeed = 10f, _acceleration = 1f, _rotationSpeed = 180f, _deceleration = 5f;

        public float MaxSpeed => _maxSpeed;
        public float Acceleration => _acceleration;
        public float Deceleration => _deceleration;
        public float RotationSpeed => _rotationSpeed;
    }
}