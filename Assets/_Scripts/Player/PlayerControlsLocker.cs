using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class PlayerControlsLocker : Locker
	{
		private int? _garageLockID = null;
		public PlayerControlsLocker(SignalBus signalBus) {
			signalBus.Subscribe<GarageOpenedSignal>(OnGarageOpened);
			signalBus.Subscribe<GarageClosedSignal>(OnGarageClosed);
		}
		private void OnGarageOpened()
		{
			if (_garageLockID == null) _garageLockID = CreateLock();
		}
		private void OnGarageClosed()
		{
			if (_garageLockID != null)
			{
				DeleteLock(_garageLockID.Value);
				_garageLockID = null;
			}
		}
	}
}
