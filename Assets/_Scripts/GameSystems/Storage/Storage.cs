using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Zenject;
using System.Linq;

namespace ZE.Polytrucks {
	public class Storage : IStorage
	{
		private ItemsHandler _items;

		public bool IsReadyToReceive => !IsFull;
		public bool IsEmpty => ItemsCount == 0;
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

		public bool TryAddItem(VirtualCollectable item)
		{
            if (!IsFull)
            {
                _items.Add(item);
                return true;
            }
            else return false;
        }
		public void AddItems(IList<VirtualCollectable> items, out BitArray result) => _items.AddRange(items, out result);
		public int AddItems(VirtualCollectable item, int count) => _items.Add(item, count);

        public bool TryExtractItem(VirtualCollectable item)
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
		public bool TryReserveItems(VirtualCollectable requestedItem, int count, out List<int> reserved) => _items.TryReserveItems(requestedItem, count, out reserved);
		public void RemoveReservedItems(List<int> indices) => _items.RemoveReservedItems(indices);
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
        public void ReturnItem(VirtualCollectable item) => TryAddItem(item);
        public bool TryExtractItems(VirtualCollectable item, int count) => TryExtract(item.CollectableType, item.Rarity, count);
        public bool TryExtractItems(TradeContract contract, out List<VirtualCollectable> list) => TryFormItemsList(contract, out list);       
        public int CalculateItemsCount(CollectableType type, Rarity rarity) => CalculateItemsCountOfType(type, rarity);

        public void SubscribeToItemAddEvent(Action action) => OnItemAddedEvent += action;
		public void UnsubscribeFromItemAddEvent(Action action) => OnItemAddedEvent -= action;
        public void SubscribeToItemRemoveEvent(Action action) => OnItemRemovedEvent+= action;
        public void UnsubscribeFromItemRemoveEvent(Action action) => OnItemRemovedEvent -= action;

       
        #endregion
    }
}
