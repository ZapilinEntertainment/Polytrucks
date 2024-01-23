using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class GaragePanel : MonoBehaviour
	{
		[SerializeField] private VisualItemsListController _buttonsController;
		[SerializeField] private StatsPanel _statsPanel;
		private bool _isActive = false;
		private TruckID _playerActiveTruck = TruckID.Undefined, _selectedTruck = TruckID.Undefined;
		private HangarTrucksList _trucksList;
		private Garage _observingGarage;
		private PlayerController _player;
		private SignalBus _signalBus;
		private Truck _selectedTruckModel = null;
		private IPlayerDataAgent _playerData;
		private IReadOnlyList<Sprite> _trucksIcons = null;
		public bool IsActive => _isActive;

		[Inject]
		public void Inject(HangarTrucksList trucksList, PlayerController playerController, SignalBus signalBus, IAccountDataAgent accountDataAgent)
		{
			_trucksList= trucksList;
			_player = playerController;
			_signalBus= signalBus;
			_playerData = accountDataAgent.PlayerDataAgent;
		}

        private void Start()
        {
			_buttonsController.OnItemSelectedEvent += ShowTruck;
        }


        public void Open(Garage garage)
		{
			_observingGarage= garage;
			_playerActiveTruck = (_player.ActiveVehicle as Truck).TruckConfig.TruckID;

			if (_trucksIcons == null) _trucksIcons = _trucksList.GetTruckIcons();
			int selectedTruckIndex = _trucksList.DefineTruckIndex(_playerActiveTruck, out var activeTruckConfig);
			if (activeTruckConfig != null) _selectedTruck = activeTruckConfig.TruckID;
			else _selectedTruck = TruckID.Undefined;
            _buttonsController.Setup(selectedTruckIndex, _trucksIcons);
			_statsPanel.Show(activeTruckConfig);

			SetActivity(true);
		}

		private void SetActivity(bool x)
		{
			if (x != _isActive)
			{
				_isActive = x;
				gameObject.SetActive(x);
				_observingGarage?.SetObservingStatus(x);
            }
		}

		private void ShowTruck(int index)
		{
			var info = _trucksList.GetTruckInfo(index);
			_statsPanel.Show(info.TruckConfig);
			_selectedTruckModel = _observingGarage.SpawnTruck(info.TruckPrefab);
		}
		public void BUTTON_SelectTruck()
		{
			if (_selectedTruck != TruckID.Undefined && _selectedTruckModel != null  && _playerData.TrySwitchVehicle(_selectedTruck, out var msg))
			{
				if (_selectedTruck != _playerActiveTruck)
				{
					var oldVehicle = _player.ActiveVehicle;
					_player.ChangeActiveVehicle(_selectedTruckModel);
					_selectedTruckModel = null;
					Destroy(oldVehicle);
					BUTTON_Close();
				}
			}
		}
		public void BUTTON_Close() => _signalBus.Fire<GarageClosedSignal>();

		public void Close() => SetActivity(false); // outer call
	}
}
