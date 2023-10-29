using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public struct SellOperationContainer
	{
		public int MoneyCount;
		public Rarity Rarity;
		public Vector3 SellZonePosition;

        public SellOperationContainer(int moneyCount, Rarity rarity, Vector3 sellZonePos)
		{
			MoneyCount= moneyCount;
			Rarity= rarity;
			SellZonePosition= sellZonePos;
		}
    }
}
