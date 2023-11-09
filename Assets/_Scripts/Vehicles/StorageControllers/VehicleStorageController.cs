using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public abstract class VehicleStorageController : MonoBehaviour
	{
		public abstract IStorage Storage { get; }
		public abstract Storage MainStorage { get; }
		public Action OnVehicleStorageCompositionChangedEvent;
		public Action<IStorage> OnStorageChangedEvent;
	}
}
