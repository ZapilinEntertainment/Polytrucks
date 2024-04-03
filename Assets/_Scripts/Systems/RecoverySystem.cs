using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZE.Polytrucks {
	public sealed class RecoverySystem
	{
		private RecoveryPoint _activeRecoveryPoint = null;
		private HashSet<RecoveryPoint> _recoveryPoints = new HashSet<RecoveryPoint>();
		public void SetRecoveryPoint(RecoveryPoint point)
		{
			_activeRecoveryPoint = point;
		}

		public bool TryRecovery(Vehicle vehicle)
		{
			if (_activeRecoveryPoint != null)
			{
				vehicle.RecoveryAt(_activeRecoveryPoint);
				return true;
			}
			else return false;
		}
	}
}
