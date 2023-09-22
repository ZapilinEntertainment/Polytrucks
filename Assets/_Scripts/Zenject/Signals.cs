using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public class SessionStartSignal { }
	public class SessionStopSignal { }
	public class SessionPauseSignal { }
	public class SessionResumeSignal { }
	public class CameraViewPointSetSignal {
		public Transform Point { get; private set; }
		public CameraViewPointSetSignal(Transform point)
		{
			Point = point;
		}
	}
}
