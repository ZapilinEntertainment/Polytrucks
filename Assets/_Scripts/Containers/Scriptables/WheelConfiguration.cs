using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/WheelConfiguration")]
    public sealed class WheelConfiguration : ScriptableObject
	{
        public float SuspensionLength = 1f;
        public float SpringStrength = 100f;
        public float SpringDamper = 10f;
        public float MaxSuspensionOffset = 1f;
        public float TireMass = 1f;
        public float BrakeForce = 1000f;
        public AnimationCurve TireGripCurve;
    }
}
