using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public class PlayerUI : MonoBehaviour
	{
		[SerializeField] protected RectTransform _vehicleStatLineHost;
		[SerializeField] protected ModuleTrackPanel _fuelPanelPrefab, _integrityPanelPrefab;
		protected ModuleTrackPanel _fuelPanel, _integrityPanel;
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
			bool noVehicle = vehicle == null;
			if (!noVehicle && vehicle.TryGetFuelModule(out var fuelModule))
			{
				if (_fuelPanel == null) _fuelPanel = Instantiate(_fuelPanelPrefab, _vehicleStatLineHost);
				_fuelPanel.StartTracking(fuelModule);
			}
			else
			{
				if (_fuelPanel != null) _fuelPanel.StopTracking();
			}

			if (!noVehicle && vehicle.TryGetIntegrityModule(out var module))
			{
				if (_integrityPanel == null) _integrityPanel = Instantiate(_integrityPanelPrefab, _vehicleStatLineHost);
				_integrityPanel.StartTracking(module);
			}
			else
			{
				if (_integrityPanel != null) _integrityPanel.StopTracking();
			}
		}
	}
}
