using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface ITeleportable
	{
		public bool IsTeleporting { get; }
		public void Teleport(VirtualPoint point, Action onTeleportCompleted = null); 
	}
}
