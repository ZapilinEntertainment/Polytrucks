using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IItemReceiver 
	{
		public bool IsReadyToReceive { get; }
		public int FreeSlotsCount { get; }

		public bool TryReceive(VirtualCollectable item);
		public void ReceiveItems(ICollection<VirtualCollectable> items);
		//public void SubscribeToItemReceiving(Action<VirtualCollectable> action);
		//public void UnsubscribeFromItemReceiving(Action<VirtualCollectable> action);
	}
}
