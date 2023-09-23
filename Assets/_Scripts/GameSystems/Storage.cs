using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {



	public class Storage 
	{
		private int _capacity = 0, _storagedCount = 0, _width = 1, _length = 1, _height = 1;
		private Stack<ICollectable> _storage;

		public int Capacity => _capacity;
		public int StoragedCount => _storagedCount;
		public Action OnStorageCompositionChangedEvent;
		public CollectableType[] GetCollectableTypes()
		{
			var array = new CollectableType[Capacity];
			if (StoragedCount != 0) 
			{
				var types = _storage.ToArray();
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
			_storage = new Stack<ICollectable>(_capacity);
			_storagedCount = 0;

		}
		public bool TryCollect(ICollectable collectable)
		{
			if (_storagedCount < _capacity)
			{
				_storage.Push(collectable);
				_storagedCount++;
				OnStorageCompositionChangedEvent?.Invoke();
				return true;
			}
			else return false;
		}
	}
}
