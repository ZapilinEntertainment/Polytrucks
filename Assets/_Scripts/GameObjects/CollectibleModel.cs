using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class CollectibleModel : MonoBehaviour
	{
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private SpriteRenderer _iconRenderer;
		private Pool _pool;

        public void Setup(Mesh model, Sprite icon)
        {
            _meshFilter.sharedMesh = model;
            _iconRenderer.sprite = icon;
        }
        public void Dispose()
        {
            _pool.Despawn(this);
        }

		public class Pool : MonoMemoryPool<CollectibleModel> {
            protected override void OnCreated(CollectibleModel item)
            {
                base.OnCreated(item);
                item._pool= this;
            }
            protected override void OnSpawned(CollectibleModel item)
            {
                base.OnSpawned(item);
                item.transform.localScale = Vector3.one;
            }
        }
	}
}
