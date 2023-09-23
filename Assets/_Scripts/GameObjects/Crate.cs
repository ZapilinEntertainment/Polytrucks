using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class Crate : SessionObject, ICollectable, IPoolable
	{
        [SerializeField] private CollectableType _collectableType => CollectableType.Crate_Common;
        [SerializeField] private Collider _collider;

        private CollectibleModel _model;
        private MonoMemoryPool<Crate> _pool;
        private ColliderListSystem _collidersList;
        private CollisionHandleSystem _collisionHandleSystem;
        public CollectableType CollectableType => _collectableType;

        [Inject]
        public void Setup(ColliderListSystem colliderList, CollisionHandleSystem collisionHandleSystem)
        {
            _collidersList = colliderList;
            _collisionHandleSystem = collisionHandleSystem;
        }
        public bool Collect()
        {
            _pool.Despawn(this);
            return true;
        }

        public int GetID() => _collider.GetInstanceID();


        public void SetModel(CollectibleModel model)
        {
            _model = model;
            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
        }
        public void OnSpawned()
        {
            _collidersList.AddCollectable(this);
        }
        public void OnDespawned()
        {
            if (_model != null) _model.Dispose();
            _collidersList.RemoveCollectable(GetID());
        }
        private void OnTriggerEnter(Collider other)
        {
            if (GameSessionActive && _collidersList.TryGetCollector(other.GetInstanceID(), out var collector)) _collisionHandleSystem.HandleCollection(collector, this);
        }


        public class Pool : MonoMemoryPool<Crate> {
            protected override void OnCreated(Crate item)
            {
                base.OnCreated(item);
                item._pool = this;
            }
        }
    }
}
