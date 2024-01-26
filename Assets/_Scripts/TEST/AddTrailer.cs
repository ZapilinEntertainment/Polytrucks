using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks.TestModule {
	public sealed class AddTrailer : MonoBehaviour
	{
        [SerializeField] private TrailerID _trailerID = TrailerID.NoTrailer;
		[SerializeField] private Truck _truck;        
        private TruckSpawnService _spawnService;

        [Inject]
        public void Inject(TruckSpawnService spawnService) => _spawnService = spawnService;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                if (_spawnService.TryCreateTrailer(_trailerID, out var trailer))
                {
                    _truck.TrailerConnector.AddTrailer(trailer);
                }
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_truck.HaveTrailers) _truck.TrailerConnector.RemoveTrailer();
            }
        }
    }
}
