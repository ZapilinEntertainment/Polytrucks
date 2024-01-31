using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/IntegrityConfiguration")]
    public class IntegrityConfiguration : ScriptableObject, IIntegrityConfiguration
	{
        [field: SerializeField] public int MaxHP { get; private set; }
        [field: SerializeField] public float HpDegradeSpeed { get; private set; }
        [field: SerializeField] public float HitIncomeDamageCf { get; private set; }
        [field: SerializeField] public float HitDamageLowLimit { get; private set; }
    }
}
