using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	
    public enum GroundDataWorkMode : byte { Mud, Fluid}

    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/DeformableGroundSettings")]
    public class DeformableGroundSettings : ScriptableObject
	{
        [System.Serializable]
        public class LiquidSettings
        {
            public float InitialForceCf = 1f;
            public float WaveHeightCf = 1f;
            public float WaveGenerationTime = 0.1f;
            public float WaveDownforceCf = 0.9f;
            public float MinWaveValue = 0.01f;
        }

        [field: SerializeField] public float RestoreSpeedCf { get; private set; } = 1f;
        [field: SerializeField] public float AffectionForceCf { get; private set; } = 1f;
        [field: SerializeField] public float AffectionRadiusCf { get; private set; } = 1f;
        [field: SerializeField] public float Fluidity { get; private set; } = 1f;
        [field: SerializeField] public float MinStepLengthForChanges { get; private set; } = 0.25f;
        [field: SerializeField] public float VisualHeight { get; private set; } = 1f;
        [field: SerializeField] public LiquidSettings LiquidConfig { get; private set; }
        [field: SerializeField] public GroundDataWorkMode SmoothMode { get; private set; } = GroundDataWorkMode.Mud;

        
	}
}
