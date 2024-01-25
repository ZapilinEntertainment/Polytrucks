using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks
{
    public sealed class TradeCollidersHandler : MonoBehaviour, IColliderOwner
    {
        [field: SerializeField] public List<Collider> CollectColliders = new List<Collider>();
        private int _layer;
        public bool HasMultipleColliders => CollectColliders.Count != 1;
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
        public System.Action OnCollidersListChangedEvent;

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
    }
}
