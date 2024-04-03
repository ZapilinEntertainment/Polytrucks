using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class CollectibleModel : MonoBehaviour
	{
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private SpriteRenderer _iconRenderer;
		private Pool _pool;
        private const float SCALE_EFFECT = 0.25f, DURATION = 0.25f;

        public void Setup(Mesh model, Sprite icon, Color iconColor)
        {
            _meshFilter.sharedMesh = model;
            _iconRenderer.sprite = icon;
            _iconRenderer.color = iconColor;
        }
        public void OnAllPropertiesSet()
        {
            var cachedScale = transform.localScale; 
            transform.localScale = SCALE_EFFECT * cachedScale;
            transform.DOScale(cachedScale, DURATION);
        }
        public void Dispose()
        {
            transform.parent = null;
            transform.DOScale(SCALE_EFFECT, DURATION).OnComplete(() => _pool.Despawn(this));
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
