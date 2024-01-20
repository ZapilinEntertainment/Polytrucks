using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class GaragePanel : MonoBehaviour
	{
		[SerializeField] private VisualItemsListController _buttonsController;
		[SerializeField] private StatsPanel _statsPanel;
		private TruckID _playerActiveTruck = TruckID.Undefined, _selectedTruck = TruckID.Undefined;
		private HangarTrucksList _trucksList;
		private Garage _observingGarage;
		private PlayerController _player;
		private IReadOnlyList<Sprite> _trucksIcons = null;

		[Inject]
		public void Inject(HangarTrucksList trucksList, PlayerController playerController)
		{
			_trucksList= trucksList;
			_player = playerController;
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
			gameObject.SetActive(true);
		}

		private void ShowTruck(int index)
		{
			_statsPanel.Show(_trucksList.GetTruckConfig(index));
		}
		public void BUTTON_SelectTruck()
		{
			if (_selectedTruck != TruckID.Undefined)
			{
				//_player.ChangeActiveVehicle();
			}
		}

		public void Close()
		{
            gameObject.SetActive(false);
        }
	}
}
