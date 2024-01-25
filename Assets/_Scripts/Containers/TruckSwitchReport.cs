using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public struct TruckSwitchReport
	{
		public static TruckSwitchReport UndefinedTruckError { get; }
        public static TruckSwitchReport TruckLockedError { get; }
        public static TruckSwitchReport SwitchSucceed { get; }
    }
}
