using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class VehicleStorageController : MonoBehaviour
	{
		public abstract IStorage Storage { get; }
		public System.Action<float> OnVehicleCargoChangedEvent { get; set; }
	}
}
