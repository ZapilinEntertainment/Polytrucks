using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CollectibleSpot : MonoBehaviour
	{
		[SerializeField] private float _respawnTime = 10f, _startDelay = 0f;
		[SerializeField] private CollectableType _type;
		[SerializeField] private Rarity _rarity;
		private CollectablesSpawnManager _spawnManager;

		[Inject]
		public void Inject(CollectablesSpawnManager manager) => _spawnManager = manager;
        private IEnumerator Start()
        {
			yield return new WaitForSeconds( _startDelay );
			SpawnCrate();
        }

		private void SpawnCrate() => _spawnManager.SpawnFallingCrate(_type, _rarity, transform, OnCrateFell);
		private void OnCrateFell(Crate crate)
		{
            crate.OnCollectedEvent += OnCrateCollected;
        }

		private void OnCrateCollected()
		{
			StartCoroutine(RestockCoroutine());
		}
		private IEnumerator RestockCoroutine()
		{
			yield return new WaitForSeconds( _respawnTime );
			SpawnCrate();
		}

        private void OnDrawGizmos()
        {
			Gizmos.DrawSphere(transform.position, GameConstants.CRATE_COLLECT_RADIUS);
        }
    }
}
