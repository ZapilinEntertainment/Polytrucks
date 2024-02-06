using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Ground/GroundPassabilitySettings")]
    public class GroundPassabilitySettings : ScriptableObject
	{
        [field:SerializeField] public float Harshness { get; private set; } = 0.25f;
	}
}
