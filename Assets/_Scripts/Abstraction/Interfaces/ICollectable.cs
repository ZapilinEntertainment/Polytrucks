using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollectable
	{
        public CollectableType CollectableType { get; }
        public int GetID();		
		public bool Collect();		
	}
}
