using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[System.Serializable]
	public enum CollectableType : byte
	{
		Undefined = 0, Fruits, Metals
	}
	public static class CollectableTypeExtension
	{
		public static int AsIntMaskValue(this CollectableType type) => (1 << (int)type);
		public static bool FitsInMask(this CollectableType type, int mask) => (mask & type.AsIntMaskValue()) != 0;
	}
}
