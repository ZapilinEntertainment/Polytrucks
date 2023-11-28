using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks
{
    public sealed class TradeCollidersHandler : MonoBehaviour, IColliderOwner
    {
        [field: SerializeField] public List<Collider> CollectColliders = new List<Collider>();
        public bool HasMultipleColliders => CollectColliders.Count != 1;
        public int GetID() => CollectColliders[0].GetInstanceID();
        public int[] GetIDs()
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
    }
}
