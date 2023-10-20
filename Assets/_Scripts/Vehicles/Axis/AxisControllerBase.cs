using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxisControllerBase : MonoBehaviour
	{
        public abstract Vector3 Forward { get; }
		public abstract Vector3 Position { get; }
		public abstract Quaternion Rotation { get; }
        public void Setup()
		{
			OnSetup();
		}
		abstract protected void OnSetup();
		abstract public void Stabilize();
		abstract public void Move(float step);
		abstract public void Steer(float angle);
		abstract public void Teleport(VirtualPoint point);
	}
}
