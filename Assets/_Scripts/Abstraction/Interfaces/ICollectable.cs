using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollectable : IColliderOwner
	{
        public CollectableType CollectableType { get; }
		public Rarity Rarity { get; }
		public bool Collect();
		public VirtualCollectable ToVirtual();
	}
	[System.Serializable]
	public struct VirtualCollectable
	{
		public CollectableType CollectableType;
		public Rarity Rarity;

		public VirtualCollectable(ICollectable collectable)
		{
			CollectableType= collectable.CollectableType;
			Rarity= collectable.Rarity;
		}
		public VirtualCollectable(CollectableType type, Rarity rarity)
		{
			CollectableType= type;
			Rarity= rarity;
		}

		public bool EqualsTo(CollectibleVisualRepresentation item) => CollectableType == item.CollectableType && Rarity == item.Rarity;
	}
}
