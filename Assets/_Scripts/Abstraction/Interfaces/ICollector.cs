using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollector 
	{
		public int[] GetIDs();
		public bool TryCollect(ICollectable collectable);
	}
}
