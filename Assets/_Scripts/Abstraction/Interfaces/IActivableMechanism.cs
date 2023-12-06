using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IActivableMechanism
	{
		public bool IsActive { get; }
		public System.Action OnActivatedEvent { get; set; }
		public void Activate();
	}
}
