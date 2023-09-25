using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollectable : IColliderOwner
	{
        public CollectableType CollectableType { get; }
		public bool Collect();		
	}
}
