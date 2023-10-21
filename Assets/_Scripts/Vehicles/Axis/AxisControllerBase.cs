using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public abstract class AxisControllerBase : MonoBehaviour
	{
        public abstract Vector3 Forward { get; }
		public abstract Vector3 Position { get; }
		public abstract Quaternion Rotation { get; }
		virtual public void Setup(Vehicle vehicle) { }
		abstract public void Stabilize();
		abstract public void Move(float step);
		abstract public void Steer(float angle);
		abstract public void Teleport(VirtualPoint point);
	}
}
