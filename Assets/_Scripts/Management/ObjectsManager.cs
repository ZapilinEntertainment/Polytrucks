using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public sealed class ObjectsManager : MonoBehaviour
    {
        public static ObjectsManager Instance
        {
            get
            {
                if (Current == null) Current = new GameObject("ObjectsManager").AddComponent<ObjectsManager>();
                return Current;
            }
        }
        public static ObjectsManager Current { get; private set; }

        private Dictionary<Item, GameObject> _prefabsList = new Dictionary<Item, GameObject>();

        public GameObject LoadItemModel(Item item)
        {
            if (item.ItemType == ItemType.Unidentified) return null;
            GameObject prefab;
            if (!_prefabsList.TryGetValue(item, out prefab))
            {
                prefab = Resources.Load<GameObject>("Items/" + item.GetItemName());
                _prefabsList[item] = prefab;
            }
            return Instantiate(prefab);
        }
    }
}
