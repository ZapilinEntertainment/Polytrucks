using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxisControllerBase : MonoBehaviour, ITeleportable
	{
		private bool _isSetup = false;
		virtual public bool IsActive => _isSetup;
		public bool IsTeleporting { get; private set; } = false;
		private IAxisController _axisController = IAxisController.Default;
		protected IAxisController AxisController => _axisController;

		public abstract float Speed { get; }
        public abstract Vector3 Forward { get; }
		public abstract Vector3 Position { get; }
		public abstract Quaternion Rotation { get; }
		public void Setup(IAxisController env) {
			_axisController = env;
			OnSetup();
			_isSetup = true;
		}
		virtual protected void OnSetup() { }

        private void Start()
        {
            if (!_isSetup) Setup(IAxisController.Default);
        }

        abstract public void Stabilize();
        public abstract void Teleport(VirtualPoint point, System.Action onTeleportComplete);
    }
}
