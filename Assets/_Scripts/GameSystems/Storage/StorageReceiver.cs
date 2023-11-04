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
        
        virtual public void AssignStorage(IStorage storage)
        {
            _storage = storage;
            _storage.OnItemAddedEvent+= OnItemAddedToStorageEvent;
            _storage.OnItemRemovedEvent+= OnItemRemovedFromStorageEvent;
        }

        abstract public bool TryAddItem(VirtualCollectable item);
        abstract public int AddItems(VirtualCollectable item, int count);
        abstract public void AddItems(IList<VirtualCollectable> items, out BitArray result);

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
