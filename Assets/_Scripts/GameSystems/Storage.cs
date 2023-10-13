using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using System.Linq;

namespace ZE.Polytrucks {
	public class Storage : IStorage
	{
		private class ItemsHandler
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
			public void RemoveAt(int index)
			{
				if (_itemsCount > index)
				{
					_items.RemoveAt(index);
					_itemsCount--;
                    _storage.OnItemRemovedEvent?.Invoke();
                    _storage.OnStorageCompositionChangedEvent?.Invoke();
                }
			}
			public void Remove(ICollection<VirtualCollectable> list) => Reassign(_items.Except(list).ToList(), true);


			public void AddRange(ICollection<VirtualCollectable> list)
			{				
				_items.AddRange(list);
				OnCountChanged();
			}
			public void Reassign(List<VirtualCollectable> list, bool compositionChangeEvent)
			{
                _items = list;
				OnCountChanged(compositionChangeEvent);
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

		private int _width = 1, _length = 1, _height = 1;
		private ItemsHandler _items;

		public bool IsFull => ItemsCount == Capacity;
		public int FreeSlotsCount => Capacity - ItemsCount;
		public int Capacity => _items.Capacity;
		public int ItemsCount => _items.Count;
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }
		public VirtualCollectable[] GetContents()
		{

			var items = new VirtualCollectable[Capacity];
			int i = 0;
			while (i < ItemsCount)
			{
				items[i] = _items[i];
				i++;
			}
			while(i < Capacity)
			{
				items[i++] = new VirtualCollectable(CollectableType.Undefined, Rarity.Regular);
			}
			return items;
		}

		public Storage(int capacity)
		{
			_items = new ItemsHandler(this, capacity);
		}

        public void AddItems(ICollection<VirtualCollectable> items)
		{
			if (items.Count > FreeSlotsCount) Debug.Log("attention - wrong list size");
            _items.AddRange(items);
        }

        public bool TryStartItemTransferTo(IStorage other)
		{
			if (!other.IsFull & _items.Count > 0)
			{
				bool isFull = false, anyItemTransferred = false;
				int freeSlots = other.FreeSlotsCount, itemsCount = ItemsCount;
				var newItems = new List<VirtualCollectable>(Capacity);
				for (int i = 0; i < itemsCount; i++)
				{
					if (isFull)
					{
						newItems.Add(_items[i]);
					}
					else
					{
						if (other.TryAdd(_items[i]))
						{
							anyItemTransferred = true;
						}
						isFull = other.IsFull;
					}
				}
				_items.Reassign(newItems, false);
				if (anyItemTransferred)
				{
					OnItemRemovedEvent?.Invoke();
					OnStorageCompositionChangedEvent?.Invoke();
				}
				return anyItemTransferred;
			}
			else return false;
		}

		public bool TryAdd(VirtualCollectable collectable)
		{
			if (ItemsCount < Capacity)
			{
				_items.Add(collectable);
				return true;
			}
			else return false;
		}

		public bool TryExtract(CollectableType type, Rarity rarity, int count)
		{
            if (ItemsCount >= count)
            {
				var indicesList = new List<int>(ItemsCount);
				bool allItemsFound = false;
				int candidatesCount = 0;
				for (int i = 0; i < ItemsCount; i++)
				{
					var item = _items[i];
					if (item.CollectableType != type || item.Rarity != rarity || allItemsFound)
					{
						indicesList.Add(i);
					}
					else
					{
						candidatesCount++;
						allItemsFound = candidatesCount >= count;
					}
				}
				if (allItemsFound)
				{
					var newList = new List<VirtualCollectable>(ItemsCount - count);
					foreach (int index in indicesList) newList.Add(_items[index]);
					_items.Reassign(newList, true);
					return true;
				}
				else return false;
            }
            return false;
        }
        public bool TryFormItemsList(TradeContract contract, out List<VirtualCollectable> list)
		{
			list = new List<VirtualCollectable>();
			foreach (var item in _items)
			{
				if (contract.IsItemSuits(item)) list.Add(item);
			}
			return list.Count > 0;
        } 
		public void RemoveItems(ICollection<VirtualCollectable> list) => _items.Remove(list);
        public int CalculateItemsCountOfType(CollectableType type, Rarity rarity)
		{
			if (_items.Count == 0) return 0;
			else
			{
				int count = 0;
				foreach (var item in _items)
				{
					if (item.CollectableType == type && item.Rarity == rarity) count++;
				}
				return count;
			}
		}

        public class Factory : PlaceholderFactory<Storage>
        {
			private StorageVisualizer.Factory _visualizersFactory;
			public Factory(StorageVisualizer.Factory visFactory)
			{
				_visualizersFactory = visFactory;
			}
            public Storage Create(StorageVisualSettings settings)
            {
				var storage = new Storage(settings.Capacity);
				var visualizer = _visualizersFactory.Create();
				visualizer.Setup(storage, settings);
				return storage;
            }
        }
    }
}
