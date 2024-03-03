using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using System;

namespace ZE.Polytrucks {
	public sealed class CollectablesSpawnService
	{
		private ObjectsCreateService _createService;
		private EffectsPack.CrateFallEffect _crateFallEffect;

		[Inject]
		public void Inject(ObjectsCreateService objectsManager, EffectsPack effectsPack) {
			_createService = objectsManager;
			_crateFallEffect = effectsPack.CrateFall;
		}

		private Crate CreateCollectibleCrate(CollectableType type, Rarity rarity)
		{
            var crate = _createService.CreateCrate(type, rarity);
			return crate;
        }
		public void SpawnCrate(CollectableType type, Rarity rarity, Transform point)
		{           
			CreateCollectibleCrate(type, rarity).transform.SetPositionAndRotation(point.position,point.rotation);			
        }
        public void SpawnCollectibleFallingCrate(CollectableType type, Rarity rarity, Transform point, Action<Crate> callback = null)
        {
            var crate = CreateCollectibleCrate(type, rarity);
			Vector3 position = point.position;
            crate.transform.position = position + _crateFallEffect.SpawnHeight * Vector3.up;
            UnityEngine.Object.Instantiate(_crateFallEffect.EffectPrefab, position, Quaternion.identity);

			var action = crate.transform.DOMoveY(position.y, _crateFallEffect.FallTime);
            if (callback != null) action.OnComplete(() => callback(crate));
        }
    }
}
