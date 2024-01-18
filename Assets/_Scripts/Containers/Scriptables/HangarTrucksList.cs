using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/HangarTrucksList")]
    public sealed class HangarTrucksList : ScriptableObject
	{
		[Serializable]
		public class TruckInfo
		{
			[field: SerializeField] public TruckID ID { get; private set; }
			[field: SerializeField] public Truck TruckPrefab { get; private set; }
			[field: SerializeField] public TruckConfig TruckConfig { get; private set; }
			[field: SerializeField] public WheelConfiguration WheelsConfig { get; private set; }
			[field: SerializeField] public Sprite Icon { get; private set; }
		}

		[SerializeField] private TruckInfo[] _trucksInfo;
	}
}
