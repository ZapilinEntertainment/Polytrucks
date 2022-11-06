using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoneTrucker
{
    [CreateAssetMenu(fileName = "LT_TruckSettings", menuName = "ScriptableObjects/LT_TruckSettingsObject", order = 1)]
    public sealed class LT_TruckSettings : ScriptableObject
    {
        [SerializeField] private Vector3 _centerOfMassCorrection = new Vector3(0f, -2f, 0f);
        [SerializeField]
        private float _fullAccelerationTime = 7f, _fullSteerTime = 1f, _brakingPower = 10000f,
            _backwaySpeed = 0.5f, _maxSpeed = 100f;

        public Vector3 CenterOfMassCorrection => _centerOfMassCorrection;
        public float FullAccelerationTime => _fullAccelerationTime;
        public float FullSteerTime => _fullSteerTime;
        public float BrakingPower => _brakingPower;
        public float BackwaySpeedPercent => _backwaySpeed;
        public float MaxSpeed => _maxSpeed;
    }
}
