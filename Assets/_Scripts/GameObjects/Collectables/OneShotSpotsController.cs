using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
    

    public sealed class OneShotSpotsController : MonoBehaviour
	{
        [Serializable]
        public struct CollectableSpawnSpot
        {
            public Transform Point;
            public CollectableType ItemType;
            public Rarity ItemRarity;
        }
        [SerializeField] private CollectableSpawnSpot[] _spawnSpots;
		private CollectablesSpawnManager _spawnManager;

		[Inject]
		public void Inject(CollectablesSpawnManager manager) => _spawnManager = manager;

		public void SpawnOnSpot(CollectableSpawnSpot spot) {
		}

        private void Start()
        {
            if (_spawnSpots.Length > 0)
			{
				foreach (var spot in _spawnSpots)
				{
					_spawnManager.SpawnCrate(spot.ItemType, spot.ItemRarity, spot.Point);
				}
                var additionalPoints = FindObjectsOfType<OneShotCrateSpawner>();
                if (additionalPoints.Length > 0)
                {
                    foreach (var point in additionalPoints)
                    {
                        _spawnManager.SpawnCrate(point.ResourceType, point.Rarity, point.transform);
                    }
                }
			}
            Destroy(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_spawnSpots.Length > 0)
            {
				Gizmos.color = Color.green;
				const float size = GameConstants.DEFAULT_COLLECTABLE_SIZE * 0.5f;
                foreach (var spot in _spawnSpots)
                {
					if (spot.Point != null)
					{
						Gizmos.color = spot.ItemType.GetGizmoColor();
						Gizmos.DrawSphere(spot.Point.position, size);
					}
                }
            }
        }
#endif
    }
}
