using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ISeller : IColliderOwner
	{
		public bool TryStartSell(SellZone sellZone, int goodsMask);
	}
}
