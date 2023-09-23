using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/ObjectsPack")]
    public sealed class ObjectsPack : ScriptableObject
	{
		[field: SerializeField] public Crate CratePrefab { get; private set; }
        [field: SerializeField] public CollectibleModel CrateModel { get; private set; }
    }
}
