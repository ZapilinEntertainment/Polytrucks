using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface ICountTracker 
	{
        public void OnCountChanged(int x);
		public void OnTrackableDisposed();
	}
}
