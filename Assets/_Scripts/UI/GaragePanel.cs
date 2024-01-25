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
		private TruckID _selectedTruckID = TruckID.Undefined;
		private HangarTrucksList _trucksList;
		private Garage _observingGarage;
		private PlayerController _player;
		private SignalBus _signalBus;
		private GarageService _garageService;		
		private IReadOnlyList<Sprite> _trucksIcons = null;
		public bool IsActive => _isActive;

		[Inject]
		public void Inject(HangarTrucksList trucksList, PlayerController playerController, SignalBus signalBus, GarageService garageService)
		{
			_trucksList= trucksList;
			_player = playerController;
			_signalBus= signalBus;
			_garageService = garageService;
		}

        private void Start()
        {
			_buttonsController.OnItemSelectedEvent += ShowTruck;
        }


		// panel invokes from panels manager
        public void Open(Garage garage)
		{
			_observingGarage= garage;

			if (_trucksIcons == null) _trucksIcons = _trucksList.GetTruckIcons();
			_selectedTruckID = (_player.ActiveVehicle as Truck).TruckID;
			int selectedTruckIndex = _trucksList.DefineTruckIndex(_selectedTruckID, out var activeTruckConfig);
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
			_selectedTruckID = info.TruckID;
			_statsPanel.Show(info.TruckConfig);
			_garageService.ShowTruck(_selectedTruckID, _observingGarage.ModelPoint);
		}
		public void BUTTON_SelectTruck()
		{
			if (_garageService.TrySwitchToTruck(_selectedTruckID, out var report)) BUTTON_Close();
			else
			{
				// show info of the report
			}
        }
		public void BUTTON_Close() => _signalBus.Fire<GarageClosedSignal>();

		public void Close() => SetActivity(false); // outer call
	}
}
