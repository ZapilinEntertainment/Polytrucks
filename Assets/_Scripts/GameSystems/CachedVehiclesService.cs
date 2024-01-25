using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class CachedVehiclesService
	{
		private Dictionary<TruckID, Truck> _cachedTrucks = new Dictionary<TruckID, Truck>();

		public bool TryGetTruck(TruckID truckID, out Truck truck)
		{
			if (_cachedTrucks.TryGetValue(truckID, out truck))
			{
				_cachedTrucks.Remove(truckID);
				return true;
			}
			else return false;
		}
		public void CacheTruck(Truck truck)
		{
			TruckID key = truck.TruckID;
			if (_cachedTrucks.ContainsKey(key)) UnityEngine.Object.Destroy(truck.gameObject);
			else
			{
				_cachedTrucks.Add(key, truck);
				truck.SetVisibility(false);
			}
		}
	}
}
