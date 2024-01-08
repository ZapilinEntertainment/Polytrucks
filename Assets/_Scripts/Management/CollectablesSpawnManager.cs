using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;
using System;

namespace ZE.Polytrucks {
	public sealed class CollectablesSpawnManager
	{
		private ObjectsManager _objectsManager;
		private EffectsPack.CrateFallEffect _crateFallEffect;

		[Inject]
		public void Inject(ObjectsManager objectsManager, EffectsPack effectsPack) {
			_objectsManager = objectsManager;
			_crateFallEffect = effectsPack.CrateFall;
		}

		public void SpawnCrate(CollectableType type, Rarity rarity, Transform point)
		{
            var crate = _objectsManager.CreateCrate(type, rarity);
			crate.transform.SetPositionAndRotation(point.position,point.rotation);
        }
        public void SpawnFallingCrate(CollectableType type, Rarity rarity, Transform point, Action<Crate> callback = null)
        {
            var crate = _objectsManager.CreateCrate(type, rarity);
			Vector3 position = point.position;
            crate.transform.position = position + _crateFallEffect.SpawnHeight * Vector3.up;
            UnityEngine.Object.Instantiate(_crateFallEffect.EffectPrefab, position, Quaternion.identity);

			var action = crate.transform.DOMoveY(position.y, _crateFallEffect.FallTime);
            if (callback != null) action.OnComplete(() => callback(crate));
        }
    }
}
