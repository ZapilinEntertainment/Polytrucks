using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/VehicleViewSettings")]
    public class VehicleViewSettings : ScriptableObject, ICameraObservable
	{        
        [field:SerializeField]public float HeightViewCf { get; private set; } = 1f;
        [field: SerializeField] public float HeightSpeedOffsetCf { get; private set; } = 1f;
        [field: SerializeField] public float ZSpeedOffset { get; private set; } = 10f;
    }
}
