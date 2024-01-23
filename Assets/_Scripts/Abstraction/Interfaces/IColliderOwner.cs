using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IColliderOwner 
	{
		abstract public bool HasMultipleColliders { get; }
		public int GetColliderID();
		public int[] GetColliderIDs();
	}
}
