using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
	public sealed class GaragePanel : MonoBehaviour
	{
		[SerializeField] private VisualItemsListController _buttonsController;
		[SerializeField] private StatsPanel _statsPanel;
		[SerializeField] private GameObject _alreadySelectedLabel, _selectButton, _truckLockedLabel;
		private bool _isActive = false;
		private TruckID _selectedTruckID = TruckID.Undefined;
		private HangarTrucksList _trucksList;
		private Garage _observingGarage;
		private PlayerController _player;
		private SignalBus _signalBus;
		private TruckSwitchService _garageService;
		private IPlayerDataAgent _playerData;
		public bool IsActive => _isActive;

		[Inject]
		public void Inject(HangarTrucksList trucksList, PlayerController playerController, SignalBus signalBus,
			TruckSwitchService garageService, IAccountDataAgent accountData)
		{
			_trucksList= trucksList;
			_player = playerController;
			_signalBus= signalBus;
			_garageService = garageService;
			_playerData = accountData.PlayerDataAgent;
		}

        private void Start()
        {
			_buttonsController.OnItemSelectedEvent += ShowTruck;
        }


		// panel invokes from panels manager
        public void Open(Garage garage)
		{
			_observingGarage= garage;

			var trucksInfo = _trucksList.GetTrucksInfo();
			int count = trucksInfo.Count;
			var infoContainers = new VisualItemContainer[count];
			for (int i = 0; i < count; i++)
			{
				var info = trucksInfo[i];
				infoContainers[i] = new VisualItemContainer(info.Icon,_playerData.IsTruckUnlocked(info.TruckID));
			}

			_selectedTruckID = (_player.ActiveVehicle as Truck).TruckID;
			int selectedTruckIndex = _trucksList.DefineTruckIndex(_selectedTruckID, out var activeTruckConfig);
            _buttonsController.Setup(selectedTruckIndex, infoContainers);
			ShowTruckStats(_selectedTruckID, activeTruckConfig);

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
			ShowTruckStats(_selectedTruckID, info.TruckConfig);
			_garageService.ShowTruck(_selectedTruckID, _observingGarage.ModelPoint);
		}
		private void ShowTruckStats(TruckID id, TruckConfig config)
		{
            _statsPanel.Show(config);
			if (id == _playerData.ActiveTruckID)
			{
				_alreadySelectedLabel.SetActive(true);
				_selectButton.SetActive(false);
				_truckLockedLabel.SetActive(false);
			}
			else
			{
				if (_playerData.IsTruckUnlocked(id)) {
                    _alreadySelectedLabel.SetActive(false);
                    _selectButton.SetActive(true);
                    _truckLockedLabel.SetActive(false);
                }
				else
				{
                    _alreadySelectedLabel.SetActive(false);
                    _selectButton.SetActive(false);
                    _truckLockedLabel.SetActive(true);
                }
			}
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
