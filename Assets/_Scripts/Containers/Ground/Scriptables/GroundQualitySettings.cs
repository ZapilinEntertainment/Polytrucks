using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/GroundQualitySettings")]
    public sealed class GroundQualitySettings : ScriptableObject
	{
		[field: SerializeField] public int DeformMapResolution { get; private set; } = 64;
		[field: SerializeField] public float ClearTime { get; private set; } = 3f; 
	}
}
