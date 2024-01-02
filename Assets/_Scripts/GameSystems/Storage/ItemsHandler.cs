using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
    public class ItemsHandler
    {
        private int _capacity = 0, _itemsCount = 0;
        private Storage _storage;
        private List<VirtualCollectable> _items;

        public int Count => _itemsCount;
        public int Capacity => _capacity;

        public ItemsHandler(Storage storage, int capacity)
        {
            _storage = storage;
            _capacity = capacity;
            _items = new List<VirtualCollectable>();
        }

        public VirtualCollectable this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        public void Add(VirtualCollectable item)
        {
            _items.Add(item);
            _itemsCount++;
            _storage.OnItemAddedEvent?.Invoke();
            _storage.OnStorageCompositionChangedEvent?.Invoke();
        }
        public int Add(VirtualCollectable item, int count)
        {
            int initialCount = count;
            if (count + _itemsCount >= _capacity)
            {
                count = _capacity - _itemsCount;
                _itemsCount = _capacity;
            }
            else
            {
                _itemsCount += count;
            };
            for (int i = 0; i < count; i++)
            {
                _items.Add(item);
            }
            _storage.OnItemAddedEvent?.Invoke();
            _storage.OnStorageCompositionChangedEvent?.Invoke();
            return initialCount - count;
        }
        public void AddRange(IList<VirtualCollectable> list, out BitArray addResults)
        {
            int count = list.Count, i = 0;
            addResults = new BitArray(count, false);
            while (_itemsCount < _capacity & i < count)
            {
                _items.Add(list[i]);
                addResults[i] = true;
                i++;
            }
            OnCountChanged();
        }

        public bool TryReserveItems(VirtualCollectable requestedItem, int count, out List<int> reserved)
        {
            reserved = new List<int>();
            for (int i = 0; i < _itemsCount; i++)
            {
                if (_items[i].EqualsTo(requestedItem))
                {
                    reserved.Add(i);
                    count--;
                    if (count == 0) return true;
                }
            }
            return reserved.Count != 0;
        }
        public void RemoveReservedItems(List<int> indices)
        {
            indices.Sort();
            int delta = 0;
            foreach (int i in indices)
            {
                _items.RemoveAt(i - delta);
                delta++;
            }
            OnCountChanged(delta != 0);
        }

        public void RemoveAt(int index)
        {
            if (_itemsCount > index)
            {
                _items.RemoveAt(index);
                OnCountChanged(true);
            }
        }
        public void Remove(ICollection<VirtualCollectable> clearList)
        {
            BitArray bitmask = new BitArray(_itemsCount, true);
            foreach (var item in clearList)
            {
                for (int i = 0; i < _itemsCount; i++)
                {
                    if (!bitmask[i]) continue;
                    else
                    {
                        if (item.EqualsTo(_items[i]))
                        {
                            bitmask[i] = false;
                            break;
                        }
                    }
                }
            }
            List<VirtualCollectable> newItems = new List<VirtualCollectable>();
            for (int i = 0; i < _itemsCount; i++)
            {
                if (bitmask[i]) newItems.Add(_items[i]);
            }
            Reassign(newItems, true);
        }
       
        public void Reassign(List<VirtualCollectable> list, bool compositionChangeEvent)
        {
            _items = list;
            OnCountChanged(compositionChangeEvent);
        }
        public void Clear()
        {
            _items.Clear();
            OnCountChanged();
        }
        private void OnCountChanged(bool compositionChanged = true)
        {
            int oldCount = _itemsCount;
            _itemsCount = _items.Count;
            if (oldCount != _itemsCount)
            {
                if (oldCount > _itemsCount) _storage.OnItemRemovedEvent?.Invoke();
                else _storage.OnItemAddedEvent?.Invoke();
            }
            if (compositionChanged) _storage.OnStorageCompositionChangedEvent?.Invoke();
        }

        public IEnumerator<VirtualCollectable> GetEnumerator() => _items.GetEnumerator();
    }
}
