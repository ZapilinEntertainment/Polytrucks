using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxisControllerBase : MonoBehaviour
	{
		private bool _isSetup = false;
		virtual protected bool IsActive => _isSetup;
		protected IAxisController _axisController;

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

        private IEnumerator Start()
        {
			if (_axisController == null) yield return new WaitForSecondsRealtime(1f);
			else yield break;
            if (_axisController == null) Setup(IAxisController.Default);
        }

        abstract public void Stabilize();
		abstract public void Teleport(VirtualPoint point);
	}
}
