using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {



	public class Storage 
	{
		private int _capacity = 0, _storagedCount = 0, _width = 1, _length = 1, _height = 1;
		private List<ICollectable> _items;

		public int Capacity => _capacity;
		public int StoragedCount => _storagedCount;
		public Action OnStorageCompositionChangedEvent;
		public CollectableType[] GetCollectableTypes()
		{
			var array = new CollectableType[Capacity];
			if (StoragedCount != 0) 
			{
				var types = _items.ToArray();
				int itemsCount = types.Length;
				for (int i = 0; i < itemsCount; i++)
				{
					array[i] = types[i].CollectableType;
				}
			}
			return array;
		}

		public Storage(int capacity)
		{
			_capacity = capacity;
			_items = new List<ICollectable>(_capacity);
			_storagedCount = 0;

		}
		public bool TryCollect(ICollectable collectable)
		{
			if (_storagedCount < _capacity)
			{
				_items.Add(collectable);
				_storagedCount++;
				OnStorageCompositionChangedEvent?.Invoke();
				return true;
			}
			else return false;
		}
		public bool TryStartSell(SellZone sellZone, int goodsMask)
		{
			if (StoragedCount == 0 || goodsMask == 0) return false;
			else
			{
				bool anyItemSold = false;
				List<ICollectable> newItems = new List<ICollectable>();
				foreach (var item in _items)
				{
                    if ((item.CollectableType.AsIntMaskValue() & goodsMask) != 0) SoldItem(item);
					else newItems.Add(item);
                }
				_items = newItems;
				_storagedCount = _items.Count;
				OnStorageCompositionChangedEvent?.Invoke();

                void SoldItem(ICollectable collectable)
                {
					sellZone.Sell(collectable);
                }

				return anyItemSold;
            }

            
        }
		
	}
}
