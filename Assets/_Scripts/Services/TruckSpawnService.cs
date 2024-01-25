using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class TruckSpawnService
    {
        private readonly Truck.Factory _truckFactory;
        private readonly HangarTrucksList _trucksList;
        private readonly CachedVehiclesService _cachedVehiclesService;

        public TruckSpawnService(Truck.Factory truckFactory, CachedVehiclesService cachedVehiclesService, HangarTrucksList trucksList)
        {
            _truckFactory = truckFactory;
            _cachedVehiclesService = cachedVehiclesService;
            _trucksList = trucksList;
        }

        public Truck CreateTruck(TruckID truckID)
        {
            if (_cachedVehiclesService.TryGetTruck(truckID, out var truck))
            {
                return truck;
            }
            else
            {
                if (_trucksList.TryGetTruckInfo(truckID, out var info)) return _truckFactory.Create(info.TruckPrefab);
                else return null;
            }
        }
    }
}
