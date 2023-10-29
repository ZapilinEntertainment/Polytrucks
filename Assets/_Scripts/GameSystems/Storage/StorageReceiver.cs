using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class StorageReceiver : MonoBehaviour, IItemReceiver
	{
		protected IStorage _storage;
		abstract public bool IsReadyToReceive { get; }
		abstract public int FreeSlotsCount { get; }

		abstract public void ReceiveItems(ICollection<VirtualCollectable> items);
        abstract public bool TryReceive(VirtualCollectable item);
		public void AssignStorage(IStorage storage) => _storage = storage;
        //abstract public void SubscribeToItemReceiving(Action<VirtualCollectable> action);
        //abstract public void UnsubscribeFromItemReceiving(Action<VirtualCollectable> action);
    }
}
