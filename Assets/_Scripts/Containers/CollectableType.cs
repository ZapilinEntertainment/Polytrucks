using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[System.Serializable]
	public enum CollectableType : byte
	{
		Undefined = 0, Fruits
	}
	public static class CollectableTypeExtension
	{
		public static int AsIntMaskValue(this CollectableType type) => (1 << (int)type);
	}
}
