using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class TruckSpawnService
    {
        private readonly Truck.Factory _truckFactory;
        private readonly Trailer.Factory _trailerFactory;
        private readonly HangarTrucksList _trucksList;
        private readonly CachedVehiclesService _cachedVehiclesService;

        public TruckSpawnService(Truck.Factory truckFactory, Trailer.Factory trailerFactory, CachedVehiclesService cachedVehiclesService, HangarTrucksList trucksList)
        {
            _truckFactory = truckFactory;
            _trailerFactory = trailerFactory;
            _cachedVehiclesService = cachedVehiclesService;
            _trucksList = trucksList;
        }

        public Truck CreateTruck(TruckID truckID)
        {
            Truck truck;
            if (!_cachedVehiclesService.TryGetTruck(truckID, out truck))
            {
                if (_trucksList.TryGetTruckInfo(truckID, out var info)) truck = _truckFactory.Create(info.TruckPrefab);
            }
            return truck;
        }
        public GameObject CreatePlaceholder()
        {
            if (_cachedVehiclesService.TryGetTruckPlaceholder(out var obj)) return obj;
            else
            {
                return Object.Instantiate(_trucksList.LockedTruckPlaceholder);
            }
        }

        public void CheckForTrailer(Truck truck)
        {
            if (truck.TruckConfig.TrailerRequired && !truck.HaveTrailers)
            {
                truck.StartCoroutine(ConnectTrailer(truck, truck.TruckConfig.TrailerID));
            }
        }
        public IEnumerator ConnectTrailer(Truck truck, TrailerID trailerID)
        {
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();
            if (TryCreateTrailer(trailerID, out var trailer))
            {
                yield return new WaitForFixedUpdate();
                truck.TrailerConnector.AddTrailer(trailer);
            }
        }

        public bool TryCreateTrailer(TrailerID id, out Trailer trailer)
        {
            if (!_cachedVehiclesService.TryGetTrailer(id, out trailer))
            {
                if (_trucksList.TryGetTrailerInfo(id, out var info))
                {
                    trailer = _trailerFactory.Create(info.TrailerPrefab);
                }
                else
                {
                    trailer = null;
                    return false;
                }
            }
            return trailer != null;
        }
    }
}
