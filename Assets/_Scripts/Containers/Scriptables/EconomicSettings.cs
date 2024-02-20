using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    [CreateAssetMenu(menuName = "ScriptableObjects/GameSettings/EconomicSettings")]
    public sealed class EconomicSettings : ScriptableObject
	{
		[SerializeField]
		private RarityDefinedValues<int> _baseCosts = new RarityDefinedValues<int>();
		[SerializeField]
		public TruckCost[] TruckCosts;	


		public int GetCost(Rarity rarity) => _baseCosts[rarity];
	}

	[System.Serializable]
	public struct TruckCost
	{
		public TruckID TruckID;
		public int Cost;
	}
}
