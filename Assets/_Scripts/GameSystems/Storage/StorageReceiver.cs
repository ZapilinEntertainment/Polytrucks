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
        protected Action OnItemAddedToStorageEvent, OnItemRemovedFromStorageEvent;

		abstract public void ReceiveItems(ICollection<VirtualCollectable> items);
        abstract public bool TryReceive(VirtualCollectable item);
        virtual public void AssignStorage(IStorage storage)
        {
            _storage = storage;
            _storage.OnItemAddedEvent+= OnItemAddedToStorageEvent;
            _storage.OnItemRemovedEvent+= OnItemRemovedFromStorageEvent;
        }

        public void SubscribeToItemAddEvent(Action action)
        {
            OnItemAddedToStorageEvent += action;
        }

        public void UnsubscribeFromItemAddEvent(Action action)
        {
            OnItemAddedToStorageEvent -= action;
        }

        public void SubscribeToItemRemoveEvent(Action action)
        {
            OnItemRemovedFromStorageEvent += action;
        }

        public void UnsubscribeFromItemRemoveEvent(Action action)
        {
            OnItemRemovedFromStorageEvent -= action;
        }
        //abstract public void SubscribeToItemReceiving(Action<VirtualCollectable> action);
        //abstract public void UnsubscribeFromItemReceiving(Action<VirtualCollectable> action);
    }
}
