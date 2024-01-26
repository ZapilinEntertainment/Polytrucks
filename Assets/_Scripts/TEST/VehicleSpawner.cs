using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.TestModule {
	public sealed class VehicleSpawner : MonoBehaviour
	{
		[SerializeField] private TruckID _truckID = TruckID.TractorRosa;
		[SerializeField] private bool _withTrailer = false;
		private TruckSpawnService _truckSpawn;
		[Inject]
		public void Inject(TruckSpawnService truckSpawnService)
		{
			_truckSpawn= truckSpawnService;
		}

        private void Start()
        {
			var truck = _truckSpawn.CreateTruck(_truckID);
			truck.Teleport(new VirtualPoint(transform), () => AddTrailer(truck));		
        }
		private void AddTrailer(Truck truck)
		{
            if (_withTrailer && _truckSpawn.TryCreateTrailer(truck.TruckConfig.TrailerID, out var trailer))
            {
				trailer.Teleport(truck.TrailerConnector.CalculateTrailerPosition(5f), () => OnTrailerTeleported(truck, trailer));
               // truck.TrailerConnector.AddTrailer(trailer);
            }
        }
		private void OnTrailerTeleported(Truck truck, Trailer trailer)
		{
			VirtualPoint pos = truck.FormVirtualPoint();

			trailer.Teleport(new VirtualPoint(pos.Position + Vector3.back * 5f, truck.Rigidbody.rotation), null);
			truck.Teleport(pos);
        }
    }
}
