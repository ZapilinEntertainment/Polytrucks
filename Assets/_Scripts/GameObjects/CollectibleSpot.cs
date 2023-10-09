using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CollectibleSpot : MonoBehaviour
	{
		[SerializeField] private float _respawnTime = 10f;
		[SerializeField] private CollectableType _type;
		[SerializeField] private Rarity _rarity;
		private ObjectsManager _objectsManager;

		[Inject]
		public void Inject(ObjectsManager objectsManager)
		{
			_objectsManager= objectsManager;
		}

        private void Start()
        {
			SpawnCrate();
        }

		private void SpawnCrate()
		{
            var crate = _objectsManager.CreateCrate(_type,_rarity);
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
