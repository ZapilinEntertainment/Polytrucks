using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ITrailerConnectionPoint
	{
		public Rigidbody Rigidbody { get; }
		public VirtualPoint CalculateTrailerPosition(float distance);
	}
}
