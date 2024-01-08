using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[System.Serializable]
	public enum CollectableType : byte
	{
		Undefined = 0, Fruits, Metals, Lumber
	}
	public static class CollectableTypeExtension
	{
		public static int AsIntMaskValue(this CollectableType type) => (1 << (int)type);
		public static bool FitsInMask(this CollectableType type, int mask) => (mask & type.AsIntMaskValue()) != 0;
		public static Color GetGizmoColor(this CollectableType type)
		{
			switch (type)
			{
				case CollectableType.Lumber: return new Color(0.52f, 0.39f, 0.13f);
				case CollectableType.Fruits: return Color.green;
				case CollectableType.Metals: return Color.black;
				default: return Color.white;
			}
		}
	}
}
