using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public interface ICountTrackable 
	{
		public int CollectedCount { get; }	
		public int TargetCount { get; }
		public void Subscribe(ICountTracker tracker);
		public void Unsubscribe(ICountTracker tracker);
	}
}
