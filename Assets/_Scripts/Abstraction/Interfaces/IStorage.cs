using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IStorage : IItemProvider, IItemReceiver
	{
        public bool IsEmpty => ItemsCount == 0;
        public bool IsFull => ItemsCount == Capacity;        
        public int ItemsCount { get; }
        public int Capacity { get; }        
        public Action OnItemAddedEvent { get; set; }
        public Action OnItemRemovedEvent { get; set; }
        public Action OnStorageCompositionChangedEvent { get; set; }

        public void MakeEmpty();
        public bool TryFormItemsList(TradeContract contract, out List<VirtualCollectable> list);
        public bool TryLoadCargo(VirtualCollectable item, int count);
        public VirtualCollectable[] GetContents();
    }
}
