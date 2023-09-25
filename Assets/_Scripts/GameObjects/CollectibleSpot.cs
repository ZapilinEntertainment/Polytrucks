using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CollectibleSpot : MonoBehaviour
	{
		[SerializeField] private float _respawnTime = 10f;
		private ObjectsManager _objectsManager;

		[Inject]
		public void Setup(ObjectsManager objectsManager)
		{
			_objectsManager= objectsManager;
		}

        private void Start()
        {
			SpawnCrate();
        }

		private void SpawnCrate()
		{
            var crate = _objectsManager.CreateCrate();
            crate.transform.position = transform.position;
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
