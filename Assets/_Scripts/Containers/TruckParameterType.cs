using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public enum TruckParameterType 
	{
	   Undefined, MaxSpeed, Acceleration, Mass, Passability, Capacity
	}
	public static class TruckParameterTypeExtension
	{
		public static float GetMaxValue(this TruckParameterType type)
		{
			switch (type)
			{
				case TruckParameterType.MaxSpeed: return 100f;
				case TruckParameterType.Mass: return 500f;
				default: return 1f;
			}
		}
	}
}
