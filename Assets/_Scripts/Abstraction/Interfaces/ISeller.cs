using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISeller : IColliderOwner
	{
		public bool TryStartSell(ISellZone sellZone, int goodsMask, RarityConditions rarity);
	}
}
