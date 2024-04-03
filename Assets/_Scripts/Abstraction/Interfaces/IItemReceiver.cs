using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IItemReceiver 
	{
		public bool IsReadyToReceive { get; }
		public int FreeSlotsCount { get; }

		public bool TryAddItem(VirtualCollectable item);
        public int AddItems(VirtualCollectable item, int count); 
        /// <summary>
        /// returns residue
        /// </summary>
		public void AddItems(IReadOnlyList<VirtualCollectable> items, out BitArray result);
        //public void SubscribeToItemReceiving(Action<VirtualCollectable> action);
        //public void UnsubscribeFromItemReceiving(Action<VirtualCollectable> action);

        public void SubscribeToItemAddEvent(Action action);
        public void UnsubscribeFromItemAddEvent(Action action);
        public void SubscribeToItemRemoveEvent(Action action);
        public void UnsubscribeFromItemRemoveEvent(Action action);
    }
}
