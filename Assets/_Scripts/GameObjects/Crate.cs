using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks {
	public sealed class Crate : SessionObject, ICollectable, IPoolable
	{
        [SerializeField] private CollectableType _collectableType = CollectableType.Undefined;
        [SerializeField] private Rarity _rarity = Rarity.Regular;
        [SerializeField] private Collider _collider;

        private CollectibleModel _model;
        private MonoMemoryPool<Crate> _pool;
        private ColliderListSystem _collidersList;
        private CollisionHandleSystem _collisionHandleSystem;
        private CollectablesManager _collectiblesManager;

        public Action OnCollectedEvent { get; set; }

        public bool HaveMultipleColliders => false;
        public CollectableType CollectableType => _collectableType;
        public Rarity Rarity => _rarity;
        public int GetColliderID() => _collider.GetInstanceID();
        public int[] GetColliderIDs() => new int[1] { GetColliderID() };

        [Inject]
        public void Inject(ColliderListSystem colliderList, CollisionHandleSystem collisionHandleSystem, CollectablesManager manager)
        {
            _collidersList = colliderList;
            _collisionHandleSystem = collisionHandleSystem;
            _collectiblesManager = manager; 
        }
        public bool Collect()
        {
            _pool.Despawn(this);
            OnCollectedEvent?.Invoke();
            OnCollectedEvent = null;
            return true;
        }
        public VirtualCollectable ToVirtual() => new VirtualCollectable(this);

        

        public void Setup(CollectableType type, Rarity rarity, CollectibleModel model)
        {
            _collectableType= type;
            _rarity= rarity;

            _model = model;
            model.transform.parent = transform;
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
        }
        public void OnSpawned()
        {
            _collidersList.AddCollectable(this);
            _collectiblesManager.AddCollectable(this);
        }
        public void OnDespawned()
        {
            if (_model != null) _model.Dispose();
            _collidersList.RemoveCollectable(this);
            _collectiblesManager.RemoveCollectable(this);
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
