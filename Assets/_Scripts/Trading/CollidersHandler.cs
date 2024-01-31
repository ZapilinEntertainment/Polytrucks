using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;

namespace ZE.Polytrucks
{
    public sealed class CollidersHandler : MonoBehaviour, IColliderOwner
    {
        private class CollisionEventsModule
        {
            private readonly int _collisionEventsMask = 0;
            private readonly CollidersHandler _host;
            private CollisionResult _currentSituation = null;

            public CollisionEventsModule(CollidersHandler host, int collisionEventsMask)
            {
                _host = host;
                _collisionEventsMask = collisionEventsMask;
                _host.OnCollidersListChangedEvent += OnCollidersListChanged;
                OnCollidersListChanged();
            }

            public void Update()
            {
                if (_currentSituation != null)
                {
                    _host.OnVehicleHitEvent?.Invoke(_currentSituation);
                    _currentSituation = null;
                }
            }
            private void OnCollisionEvent(CollisionResult result)
            {
                if (_currentSituation == null) _currentSituation = result;
                else
                {
                    if (_currentSituation.Impulse < result.Impulse) _currentSituation = result;
                }
            }
            private void OnCollidersListChanged()
            {
                foreach (var collider in _host.CollectColliders)
                {
                    CollisionDetector detector;
                    if (!collider.TryGetComponent<CollisionDetector>(out detector))
                    {
                        detector = collider.gameObject.AddComponent<CollisionDetector>();
                    }
                    detector.Setup(_collisionEventsMask, OnCollisionEvent);
                }
            }
        }

        [field: SerializeField] public List<Collider> CollectColliders = new List<Collider>();
        private bool _eventsModulePresented = false;
        private int _layer;
        private CollisionEventsModule _eventsModule = null;
        private Action<CollisionResult> OnVehicleHitEvent;
        public bool HaveMultipleColliders => CollectColliders.Count != 1;
        public int GetColliderID() => CollectColliders[0].GetInstanceID();
        public int[] GetColliderIDs()
        {
            int count = CollectColliders.Count;
            var ids = new int[count];
            for (int i = 0; i < count; i++)
            {
                ids[i] = CollectColliders[i].GetInstanceID();
            }
            return ids;
        }
        public Action OnCollidersListChangedEvent;


        public void AddCollider(Collider collider)
        {
            collider.gameObject.layer = _layer;
            CollectColliders.Add(collider);
            OnCollidersListChangedEvent?.Invoke();
        }

        public IReadOnlyCollection<Vector3> GetBounds()
        {
            var list = new List<Vector3>();
            foreach (var collider in CollectColliders)
            {
                list.Add(collider.bounds.max);
                list.Add(collider.bounds.min);
            }
            return list;
        }
        public void SetColliderActivity(bool x)
        {
            foreach (var collider in CollectColliders) { collider.enabled = x; }
        }
        public void SetLayer(int x)
        {
            foreach (var collider in CollectColliders) collider.gameObject.layer = x;
        }

        private void FixedUpdate()
        {
            if (_eventsModulePresented) _eventsModule.Update();
        }

        public void SubscribeToHits(IVehicleHitEventSubscriber listener)
        {
            if (_eventsModule == null)
            {
                _eventsModule = new CollisionEventsModule(this, GameConstants.GetCustomLayermask(CustomLayermask.VehicleDamageMask));
                _eventsModulePresented = true;
            }
            OnVehicleHitEvent += listener.OnVehicleHit;
        }
        public void UnsubscribeFromHits(IVehicleHitEventSubscriber listener)
        {
            OnVehicleHitEvent-= listener.OnVehicleHit;
        }
    }
    public interface IVehicleHitEventSubscriber {
        public void OnVehicleHit(CollisionResult result);
    }
}
