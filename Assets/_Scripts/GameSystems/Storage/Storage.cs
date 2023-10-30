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
			public int Add(VirtualCollectable item, int count)
			{
				int initialCount = count ;
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
		private ItemsHandler _items;

		public bool IsReadyToReceive => !IsFull;
		public bool IsFull => ItemsCount == Capacity;
		public int FreeSlotsCount => Capacity - ItemsCount;
		public int Capacity => _items.Capacity;
		public int ItemsCount => _items.Count;
		public float CargoMass => ItemsCount * GameConstants.BASE_CRATE_MASS;
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }

		public int AvailableItemsCount => ItemsCount;

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


		public bool TryReceive(VirtualCollectable item) => TryAdd(item);
		public void ReceiveItems(ICollection<VirtualCollectable> items) => AddItems(items);
		public bool TryAdd(VirtualCollectable collectable)
		{
			if (!IsFull)
			{
				_items.Add(collectable);
				return true;
			}
			else return false;
		}

		/// <returns> residue </returns>
		public int TryAdd(VirtualCollectable collectable, int count)
		{
			if (IsFull) return count;
			else
			{
				return _items.Add(collectable, count);
			}
		}

		public bool TryExtract(VirtualCollectable item)
		{
            for (int i = 0; i < ItemsCount; i++)
            {
				if (_items[i].EqualsTo(item))
				{
					_items.RemoveAt(i);
					return true;
				}
            }
			return false;
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
			int count = 0;
			foreach (var item in _items)
			{
				if (contract.IsItemSuits(item))
				{
					list.Add(item);
					count++;
					if (count >= contract.MaxCount) break;
				}
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

        #region item provider
        public void SubscribeToProvisionListChange(Action action)
		{
			OnItemAddedEvent += action;
		}
        public void UnsubscribeFromProvisionListChange(Action action)
		{
			OnItemAddedEvent -= action;
		}
        public void ReturnItem(VirtualCollectable item) => TryAdd(item);
		public bool TryProvideItem(VirtualCollectable item) => TryExtract(item);
        public bool TryProvideItems(VirtualCollectable item, int count) => TryExtract(item.CollectableType, item.Rarity, count);
        public bool TryProvideItems(TradeContract contract, out List<VirtualCollectable> list) => TryFormItemsList(contract, out list);       
        public int CalculateItemsCount(CollectableType type, Rarity rarity) => CalculateItemsCountOfType(type, rarity);

        public void SubscribeToItemAddEvent(Action action) => OnItemAddedEvent += action;
		public void UnsubscribeFromItemAddEvent(Action action) => OnItemAddedEvent -= action;
        public void SubscribeToItemRemoveEvent(Action action) => OnItemRemovedEvent+= action;
        public void UnsubscribeFromItemRemoveEvent(Action action) => OnItemRemovedEvent -= action;
        #endregion

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
