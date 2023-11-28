using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxisControllerBase : MonoBehaviour
	{
		private bool _isSetup = false;
		protected bool IsActive => _isSetup;
		protected Truck Truck { get; private set; }
		protected TruckConfig Config { get; private set; }
        public abstract Vector3 Forward { get; }
		public abstract Vector3 Position { get; }
		public abstract Quaternion Rotation { get; }
		public void Setup(Truck truck) {
			Truck = truck;
			Config = truck.TruckConfig;
			OnSetup();
			_isSetup = true;
		}
		virtual protected void OnSetup() { }

		abstract public void Stabilize();
		abstract public void Teleport(VirtualPoint point);
	}
}
