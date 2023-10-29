using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface IItemReceiver 
	{
		public bool IsReadyToReceive { get; }
	}
}
