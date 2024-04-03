using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IFuelConfiguration 
	{
		public float TankVolume { get; }
		public float FuelConsumption { get; }
	}
}
