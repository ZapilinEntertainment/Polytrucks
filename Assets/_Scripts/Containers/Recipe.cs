using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	[System.Serializable]
	public struct Recipe
	{
		public CollectableType Input;
		public Rarity InputRarity;
		public int InputValue;
		[Space]
		public CollectableType Output;
		public Rarity OutputRarity;
		public int OutputValue;
		[Space]
		public float ProductionTime;

		public VirtualCollectable ResultItem => new VirtualCollectable(Output, OutputRarity);
	}
}
