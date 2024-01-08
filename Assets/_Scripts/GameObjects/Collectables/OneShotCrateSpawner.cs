using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class OneShotCrateSpawner : MonoBehaviour
	{
		[field: SerializeField] public CollectableType ResourceType { get;private set; }
		[field: SerializeField] public Rarity Rarity { get;private set; }		
	}
}
