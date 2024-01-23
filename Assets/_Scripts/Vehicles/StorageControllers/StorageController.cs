using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public abstract class StorageController : MonoBehaviour, IStorageController
	{
		public abstract IStorage Storage { get; }
		public abstract Storage MainStorage { get; }
		public Action OnStorageCompositionChangedEvent;

		public abstract void SetInitialStorageConfig(VisualStorageSettings config);
	}
}
