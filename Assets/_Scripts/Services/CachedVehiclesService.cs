using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {

	public interface ICachableVehicle 
	{
		public GameObject gameObject { get; }
        public void SetVisibility(bool x);
	}
	public class CachedVehiclesService
	{
		protected class VehicleCacher<T2> where T2 : ICachableVehicle
        {
			private Dictionary<int, T2> _list = new();

            public bool TryGetCachable(int key, out T2 vehicle)
            {
                if (_list == null)
                {
                    vehicle = default;
                    return false;
                }
                else
                {
                    if (_list.TryGetValue(key, out vehicle))
                    {
                        _list.Remove(key);
                        return true;
                    }
                    else return false;
                }
            }
            public void CacheVehicle(int key, T2 vehicle)
            {
                if (_list.ContainsKey(key)) UnityEngine.Object.Destroy(vehicle.gameObject);
                else
                {
                    _list.Add(key, vehicle);
                    vehicle.SetVisibility(false);
                }
            }
        }

        private GameObject _cachedTruckPlaceholder = null;
		private VehicleCacher<Truck> _trucksCacher;
		private VehicleCacher<Trailer> _trailersCacher;
		protected VehicleCacher<Truck> TrucksCacher { get
			{
				if (_trucksCacher == null) _trucksCacher = new VehicleCacher<Truck>();
				return _trucksCacher;
			} }
        protected VehicleCacher<Trailer> TrailersCacher
        {
            get
            {
                if (_trailersCacher == null) _trailersCacher = new VehicleCacher<Trailer>();
                return _trailersCacher;
            }
        }


        public bool TryGetTruckPlaceholder(out GameObject placeholder)
        {
            if (_cachedTruckPlaceholder != null)
            {
                placeholder = _cachedTruckPlaceholder;
                return true;
            }
            else
            {
                placeholder = null;
                return false;
            }
        }
        public void CachePlaceholder(GameObject placeholder)
        {
            if (_cachedTruckPlaceholder != placeholder) GameObject.Destroy(_cachedTruckPlaceholder);
            _cachedTruckPlaceholder = placeholder;
            _cachedTruckPlaceholder.SetActive(false);
        }

        public bool TryGetTruck(TruckID truckID, out Truck truck)
        {
            if (_trucksCacher == null)
            {
                truck = null;
                return false;
            }
            else return _trucksCacher.TryGetCachable((int)truckID, out truck);
        }
        public void CacheTruck(Truck truck) => TrucksCacher.CacheVehicle((int)truck.TruckID, truck);
        public bool TryGetTrailer(TrailerID trailerID, out Trailer trailer)
        {
            if (_trailersCacher == null)
            {
                trailer = null;
                return false;
            }
            else return _trailersCacher.TryGetCachable((int)trailerID, out trailer);
        }
        public void CacheTrailer(Trailer trailer) => TrailersCacher.CacheVehicle((int)trailer.TrailerID, trailer);
	}
}
