using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.TestModule {
	public sealed class VehicleSpawner : MonoBehaviour
	{
		[SerializeField] private TruckID _truckID = TruckID.TractorRosa;
		private TruckSpawnService _truckSpawn;
		[Inject]
		public void Inject(TruckSpawnService truckSpawnService)
		{
			_truckSpawn= truckSpawnService;
		}

        private void Start()
        {
			var truck = _truckSpawn.CreateTruck(_truckID);
			truck.Teleport(new VirtualPoint(transform));
        }
    }
}
