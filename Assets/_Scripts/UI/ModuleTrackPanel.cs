using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ZE.Polytrucks {
	public sealed class ModuleTrackPanel : VehicleModuleTracker<FuelModule>
	{		
		[SerializeField] private ProgressionBar _progressionBar;
		private bool _isTracking = false;
		private ITrackableVehicleModule _trackingModule = null;

		public void StartTracking(ITrackableVehicleModule module)
		{
			_isTracking = true;
			_trackingModule = module;
			_trackingModule.OnModuleDisposedEvent += StopTracking;
			ChangeStatus(TrackerStatus.Appearing);
		}
		public void StopTracking()
		{
			if (_trackingModule != null)
			{
				_trackingModule.OnModuleDisposedEvent-= StopTracking;
				_trackingModule = null;
			}
			ChangeStatus(TrackerStatus.Disabling);
			_isTracking = false;
		}

        private void Update()
        {
            if (_isTracking)
			{
				_progressionBar.SetProgress(_trackingModule.MeaningValue);
			}
        }
    }
}
