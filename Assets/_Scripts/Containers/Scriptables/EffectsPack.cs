using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

    public enum EffectType : byte { Undefined, CompleteEffect, DirtEffect }

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
		[field:SerializeField] public ParticleSystem CompleteEffectPrefab { get; private set; }
		[field: SerializeField] public ParticleSystem DirtEffectPrefab { get; private set; }


		public ParticleSystem GetEffectPrefab(EffectType type)
		{
			switch (type)
			{
				case EffectType.DirtEffect: return DirtEffectPrefab;
				case EffectType.CompleteEffect: return CompleteEffectPrefab;
				default:return null;
			}
		}
	}
}
