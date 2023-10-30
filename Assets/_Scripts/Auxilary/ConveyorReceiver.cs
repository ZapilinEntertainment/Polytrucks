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

        public override void ReceiveItems(ICollection<VirtualCollectable> items) => _conveyor.ReceiveItems(items);
        public override bool TryReceive(VirtualCollectable item)
        {
            if (_conveyor.TryReceive(item))
            {
                //OnItemReceivedEvent?.Invoke(item);
                return true;
            }
            else return false;
        }
        public override void AssignStorage(IStorage storage)
        {
            base.AssignStorage(storage);
            _conveyor.AssignReceiver(storage);
        }
    }
}
