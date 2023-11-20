using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/EffectsPack")]
    public sealed class EffectsPack : ScriptableObject
	{
		[Serializable]
		public class CrateFallEffect
		{
			[field: SerializeField] public ParticleSystem EffectPrefab { get; private set; }
			[field: SerializeField] public float SpawnHeight { get; private set; } = 2f;
			[field: SerializeField] public float FallTime { get; private set; } = 1f;
		}

		[field: SerializeField] public CrateFallEffect CrateFall { get; private set; }
	}
}
