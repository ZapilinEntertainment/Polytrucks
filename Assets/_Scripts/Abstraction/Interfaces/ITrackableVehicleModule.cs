using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ITrackableVehicleModule
	{
		public float MeaningValue { get; }
		public System.Action OnModuleDisposedEvent { get; set; }
	}
}
