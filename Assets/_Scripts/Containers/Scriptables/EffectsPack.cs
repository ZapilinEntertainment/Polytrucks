using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

    public enum EffectType : byte { Undefined, CompleteEffect, MudEffect, DirtDust, GrassDust }

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
		[field: SerializeField] public ParticleSystem MudEffectPrefab { get; private set; }
		[field: SerializeField] public ParticleSystem GrassDustPrefab { get; private set; }
		[field: SerializeField] public ParticleSystem DirtDustPrefab { get; private set; }


		public ParticleSystem GetEffectPrefab(EffectType type)
		{
			switch (type)
			{
				case EffectType.MudEffect: return MudEffectPrefab;
				case EffectType.CompleteEffect: return CompleteEffectPrefab;
				case EffectType.DirtDust: return DirtDustPrefab;
				case EffectType.GrassDust: return GrassDustPrefab;
				default:return null;
			}
		}
	}
}
