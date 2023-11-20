using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class CollectibleSpot : MonoBehaviour
	{
		[SerializeField] private float _respawnTime = 10f, _startDelay = 0f;
		[SerializeField] private CollectableType _type;
		[SerializeField] private Rarity _rarity;
		private ObjectsManager _objectsManager;
		private EffectsPack.CrateFallEffect _crateFallEffect;

		[Inject]
		public void Inject(ObjectsManager objectsManager, EffectsPack effectsPack)
		{
			_objectsManager= objectsManager;
			_crateFallEffect= effectsPack.CrateFall;
		}

        private IEnumerator Start()
        {
			yield return new WaitForSeconds( _startDelay );
			SpawnCrate();
        }

		private void SpawnCrate()
		{
            var crate = _objectsManager.CreateCrate(_type,_rarity);
            crate.transform.position = transform.position + _crateFallEffect.SpawnHeight * Vector3.up;
			Instantiate(_crateFallEffect.EffectPrefab, transform.position, Quaternion.identity);
			crate.transform.DOMoveY(transform.position.y, _crateFallEffect.FallTime).OnComplete(() => OnCrateFell(crate));		
        }
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
