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
				var point = truck.TrailerConnector.CalculateTrailerPosition(trailer.ConnectDistance);
                //trailer.Teleport(point, () => truck.TrailerConnector.AddTrailer(trailer));
                truck.TrailerConnector.AddTrailer(trailer)
                ;
				

				//StartCoroutine(testcor(truck));
            }
        }
		private IEnumerator testcor(Truck truck)
		{
			yield return new WaitForSeconds(1f);
			var point = truck.FormVirtualPoint();

			var truck2 = _truckSpawn.CreateTruck(TruckID.TruckRobert);
			truck2.Teleport(new VirtualPoint(point.Position + Vector3.left * 3f + Vector3.up, point.Rotation));

			if ( _truckSpawn.TryCreateTrailer(TrailerID.FarmerTrailer, out var trailer))
			{
				trailer.Teleport(new VirtualPoint(point.Position + Vector3.right * 3f + Vector3.up, point.Rotation));
				trailer.gameObject.SetActive(false);
			}

			
        }
    }
}
