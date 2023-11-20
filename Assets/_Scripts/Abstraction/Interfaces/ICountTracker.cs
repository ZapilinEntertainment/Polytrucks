using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace ZE.Polytrucks {
	public interface ICountTracker 
	{
		public void Setup(int maxCount, int currentCount = 0, bool isActive = true);
        public void OnTrackStatusChanged(bool x);
        public void OnCountChanged(int x);
		public void StopTracking();
	}
}
