using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class ReplenishableConveyor : ReplenishableStorage
	{
		[SerializeField] private ConveyorReceiver _receiver;

        protected override void Start()
        {
            var storage = PrepareStorage();
            SpawnStartItems(storage); // start items spawns to storage zone, not conveyor belt
            _receiver.AssignStorage(storage);
            AssignReceiver(_receiver);
        }
    }
}
