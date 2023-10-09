using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollector : IColliderOwner
	{
		public bool TryCollect(ICollectable collectable);
        public bool TryStartCollect(IStorage storage);
    }
}
