using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public sealed class ConveyorReceiver : StorageReceiver
	{
		[SerializeField] private ConveyorBelt _conveyor;
        public override bool IsReadyToReceive => _conveyor.IsReadyToReceive;
        public override int FreeSlotsCount => _conveyor.FreeSlotsCount + _storage.FreeSlotsCount;

        public override void AssignStorage(IStorage storage)
        {
            base.AssignStorage(storage);
            _conveyor.AssignReceiver(storage);
        }

        public override bool TryAddItem(VirtualCollectable item)
        {
            if (_conveyor.TryAddItem(item))
            {
                //OnItemReceivedEvent?.Invoke(item);
                return true;
            }
            else return false;
        }
        public override void AddItems(IReadOnlyList<VirtualCollectable> items, out BitArray result) => _conveyor.AddItems(items, out result);
        public override int AddItems(VirtualCollectable item, int count) => _conveyor.AddItems(item, count);   
    }
}
