using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] protected RectTransform _vehicleStatLineHost;
		[SerializeField] protected ModuleTrackPanel _fuelPanelPrefab;
		protected ModuleTrackPanel _fuelPanel;
		protected PlayerController _player;

		[Inject]
		public void Inject(PlayerController playerController)
		{
			_player = playerController;
            _player.OnVehicleChangedEvent += OnPlayerVehicleChanged;
		}

        private void Start()
        {
			OnPlayerVehicleChanged(_player.ActiveVehicle);
        }

        private void OnPlayerVehicleChanged(Vehicle vehicle)
		{
			if (vehicle != null && vehicle.TryGetFuelModule(out var fuelModule))
			{
				if (_fuelPanel == null) _fuelPanel = Instantiate(_fuelPanelPrefab, _vehicleStatLineHost);
				_fuelPanel.StartTracking(fuelModule);
			}
			else
			{
				if (_fuelPanel != null) _fuelPanel.StopTracking();
			}
		}
	}
}
