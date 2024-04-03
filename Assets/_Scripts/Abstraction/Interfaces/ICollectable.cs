using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICollectable : IColliderOwner
	{
        public CollectableType CollectableType { get; }
		public Rarity Rarity { get; }
		public System.Action OnCollectedEvent { get; set; }
		public bool Collect();
		public VirtualCollectable ToVirtual();
	}
	[System.Serializable]
	public struct VirtualCollectable
	{
		public CollectableType CollectableType;
		public Rarity Rarity;

        #region equality
        public override bool Equals(object obj) => obj is VirtualCollectable other && this.Equals(other);
		public bool Equals(VirtualCollectable other) => CollectableType == other.CollectableType && Rarity == other.Rarity;
        public override int GetHashCode() => (CollectableType, Rarity).GetHashCode();
        public static bool operator ==(VirtualCollectable lhs, VirtualCollectable rhs) => lhs.Equals(rhs);		
        public static bool operator !=(VirtualCollectable lhs, VirtualCollectable rhs) => !(lhs == rhs);
        public static bool operator ==(VirtualCollectable lhs, CollectibleVisualRepresentation rhs) => lhs.CollectableType == rhs.CollectableType && lhs.Rarity == rhs.Rarity;
		public static bool operator !=(VirtualCollectable lhs, CollectibleVisualRepresentation rhs) => !(lhs == rhs);
		#endregion
		public override string ToString() => $"{Rarity}_{CollectableType}";

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

		public static VirtualCollectable Empty => new (CollectableType.Undefined, Rarity.Regular);
    }
}
