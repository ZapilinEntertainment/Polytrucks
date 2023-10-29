using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface IItemProvider
	{
		public Action<VirtualCollectable> OnItemProvidedEvent { get; }
	}
}
