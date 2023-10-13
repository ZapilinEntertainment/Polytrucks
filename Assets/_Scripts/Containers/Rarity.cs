using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public enum Rarity : byte
	{
		Regular = 0,Advanced, Industrial,Rare,Mastery,Legendary,Unique
	}

	[Flags]
	public enum RarityConditions : byte
	{
		Regular = 1, Advanced = 2, Industrial = 4, Rare = 8,Mastery = 16,Legendary = 32, Unique = 64, Any = byte.MaxValue
	}
    public static class RarityConditionsExtensions
    {
        public static bool Contains(this RarityConditions conditions, Rarity rarity)
		{
			int maskVal = 1 << (int)rarity;
			return ((int)conditions & maskVal) != 0;
		}
		
    }
}
