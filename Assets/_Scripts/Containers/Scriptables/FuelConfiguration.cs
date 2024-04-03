using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public enum FuelType : byte { Undefined, TruckFuel}

    [CreateAssetMenu(menuName = "ScriptableObjects/Vehicles/FuelConfiguration")]
    public class FuelConfiguration : ScriptableObject, IFuelConfiguration
	{
		[field: SerializeField] public FuelType FuelType { get; private set; }
		[field: SerializeField] public float TankVolume { get; private set; }
        [field: SerializeField] public float FuelConsumption { get; private set; }
	}
}
