using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;

namespace ZE.Polytrucks {
	public class Storage : IStorage
	{
		private int _capacity = 0, _itemsCount = 0, _width = 1, _length = 1, _height = 1;
		private List<VirtualCollectable> _items;

		public bool IsFull => _itemsCount == _capacity;
		public int FreeSlots => Capacity - ItemsCount;
		public int Capacity => _capacity;
		public int ItemsCount => _itemsCount;
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }
		public VirtualCollectable[] GetContents()
		{
			var items = new VirtualCollectable[Capacity];
			int i = 0;
			while (i < _itemsCount)
			{
				items[i] = _items[i];
				i++;
			}
			while(i < _capacity)
			{
				items[i++] = new VirtualCollectable(CollectableType.Undefined, Rarity.Regular);
			}
			return items;
		}

		public Storage(int capacity)
		{
			_capacity = capacity;
			_items = new List<VirtualCollectable>(_capacity);
			_itemsCount = 0;
		}

		public bool TryStartItemTransferTo(IStorage other)
		{
			if (!other.IsFull & _itemsCount > 0)
			{
				bool isFull = false, anyItemTransferred = false;
				int freeSlots = other.FreeSlots, itemsCount = ItemsCount;
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
				_items = newItems;
				_itemsCount = _items.Count;
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
			if (_itemsCount < _capacity)
			{
				_items.Add(collectable);
				_itemsCount++;
				OnItemAddedEvent?.Invoke();
				OnStorageCompositionChangedEvent?.Invoke();
				return true;
			}
			else return false;
		}
		public bool TryExtract(CollectableType type, Rarity rarity)
		{
            if (ItemsCount != 0 && type != CollectableType.Undefined) { 
				for (int i = 0; i < _itemsCount; i++)
				{
					var item = _items[i];
                    if (item.CollectableType == type && item.Rarity == rarity) {
						_items.RemoveAt(i);
						_itemsCount--;
						OnItemRemovedEvent?.Invoke();
						OnStorageCompositionChangedEvent?.Invoke();
						return true;
                    }
                }
            }
			return false;
        }
		public bool TryExtract(CollectableType type, Rarity rarity, int count)
		{
            if (ItemsCount >= count)
            {
				if (count == 1) return TryExtract(type, rarity);
				else
				{
					List<int> candidates = new List<int>();
					for (int i = 0; i < _itemsCount; i++)
					{
						var item = _items[i];
						if (item.CollectableType == type && item.Rarity == rarity)
						{
							candidates.Add(i);
						}
					}
					if (candidates.Count >= count)
					{
						int delta = 0;
						for (int i = 0; i < count; i++)
						{
							_items.RemoveAt(candidates[i] + delta);
							delta--;
							_itemsCount--;
						}
						OnItemRemovedEvent?.Invoke();
						OnStorageCompositionChangedEvent?.Invoke();
						return true;
					}
					else return false;
				}
            }
            return false;
        }
        public bool TryStartSell(ISellZone sellZone, int goodsMask, RarityConditions rarity)
		{
			if (ItemsCount == 0 || goodsMask == 0) return false;
			else
			{
				bool anyItemSold = false;
				List<VirtualCollectable> newItems = new List<VirtualCollectable>();
				foreach (var item in _items)
				{
					if ((item.CollectableType.AsIntMaskValue() & goodsMask) == 0 || !rarity.Contains(item.Rarity) || !sellZone.TrySellItem(item)) newItems.Add(item);
					else anyItemSold = true;
                }
				_items = newItems;
				_itemsCount = _items.Count;
				
				if (anyItemSold)
				{
					OnItemRemovedEvent?.Invoke();
                    OnStorageCompositionChangedEvent?.Invoke();
                }

				return anyItemSold;
            }            
        }        
        public int CalculateItemsCountOfType(CollectableType type, Rarity rarity)
		{
			if (_itemsCount == 0) return 0;
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
