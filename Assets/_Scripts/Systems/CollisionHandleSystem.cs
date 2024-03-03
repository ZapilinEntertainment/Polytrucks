using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class CollisionHandleSystem 
	{
		public void HandleCollection(ICollector collector, ICollectable collectable)
		{
            if (collector.TryCollect(collectable))
			{
				
				//эффекты + ивенты
				collectable.Collect();
			}
		}
	}
}
