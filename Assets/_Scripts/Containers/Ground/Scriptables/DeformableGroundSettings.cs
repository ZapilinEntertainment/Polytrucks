using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	

    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/DeformableGroundSettings")]
    public class DeformableGroundSettings : ScriptableObject
	{
        [field: SerializeField] public float Fluidity { get; private set; } = 1f;
        [field: SerializeField] public float VisualHeight { get; private set; } = 1f;
	}
}
