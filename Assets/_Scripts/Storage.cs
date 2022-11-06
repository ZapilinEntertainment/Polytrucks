using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Polytrucks
{
    public class Storage : MonoBehaviour
    {
        [SerializeField] protected int _columnCount = 2, _rowsCount = 4, _heightCount = 1;
        private bool _isInitialized = false;
        private List<Item> _items;
        protected int _itemsCount => _isInitialized ? _items.Count : 0;
        public bool IsEmpty => _itemsCount == 0;
        public bool IsFull => _itemsCount >= _columnCount * _rowsCount * _heightCount;

        virtual public void Initialize()
        {
            _items = new List<Item>();
            _isInitialized = true;
        }

        virtual public bool TryAddItem(Item item)
        {
            if (!_isInitialized) Initialize();
            else if (IsFull) return false;
            _items.Add(item);
            return true;
        }
        virtual public Item[] SellItems(int tradeMask)
        {
            var sellList = new List<Item>();
            if (!IsEmpty) 
            {
                var removeIndices = new List<int>();
                int count = _itemsCount;
                for(int i = 0; i < count; i++)
                {
                    Item item = _items[i];
                    if ((item.ItemType.ToMask() & tradeMask) != 0)
                    {
                        removeIndices.Add(i);
                        sellList.Add(item);
                    }
                }
                if (removeIndices.Count > 0)
                {
                    int removeDelta = 0;
                    foreach (int x in removeIndices)
                    {
                        RemoveItemAtPosition(x + removeDelta);
                        removeDelta--;
                    }
                }
            }
            return sellList.ToArray();
        }

        virtual protected void RemoveItemAtPosition(int x) => _items.RemoveAt(x);
    }
}
