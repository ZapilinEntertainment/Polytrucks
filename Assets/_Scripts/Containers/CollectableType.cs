using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[System.Serializable]
	public enum CollectableType : byte
	{
		Undefined = 0, Crate_Common, Crate_Uncommon
	}
	public static class CollectableTypeExtension
	{
		public static int AsIntMaskValue(this CollectableType type) => (1 << (int)type);
	}
}
