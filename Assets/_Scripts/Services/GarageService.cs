using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace ZE.Polytrucks {
    public class GarageService
    {
        private IPlayerDataAgent _playerData;
        private PlayerController _playerController;
        private Truck _playerTruck = null, _showingTruck = null;
        private TruckSpawnService _truckSpawnService;
        private CachedVehiclesService _cachedVehiclesManager;
        public GarageService(PlayerController playerController, TruckSpawnService truckSpawnService, CachedVehiclesService cachedVehiclesManager,
            SignalBus signalBus,IAccountDataAgent accountDataAgent)
        {
            _playerController = playerController;
            _truckSpawnService = truckSpawnService;
            _cachedVehiclesManager = cachedVehiclesManager;
            _playerData = accountDataAgent.PlayerDataAgent;

            signalBus.Subscribe<GarageOpenedSignal>(OnGarageOpened);
            signalBus.Subscribe<GarageClosedSignal>(OnGarageClosed);
        }

        private void OnGarageOpened()
        {
            _playerTruck = (_playerController.ActiveVehicle as Truck);
            _showingTruck = _playerTruck;
        }
        private void OnGarageClosed()
        {
            if (_showingTruck != _playerTruck)
            {
                _cachedVehiclesManager.CacheTruck(_showingTruck);
                _playerTruck.SetVisibility(true);
                _showingTruck = _playerTruck;
            }
        }

        public Truck ShowTruck(TruckID truckID, VirtualPoint point)
        {
            Debug.Log($"{_showingTruck.TruckID} -> {truckID}");
            if (_showingTruck != null)
            {
                if (_showingTruck.TruckID != truckID)
                {
                    if (_showingTruck == _playerTruck)
                    {
                        _playerTruck.SetVisibility(false);
                    }
                    else
                    {
                        var truckLink = _showingTruck;
                        _cachedVehiclesManager.CacheTruck(truckLink);
                        _showingTruck = null;
                    }
                }
                else return _showingTruck;
            }

            if (truckID == _playerTruck.TruckID)
            {
                _showingTruck = _playerTruck;
            }
            else
            {
                _showingTruck= _truckSpawnService.CreateTruck(truckID);                
            }
            _showingTruck.SetVisibility(true);
            _showingTruck.Teleport(point);
            return _showingTruck;
        }

        public bool TrySwitchToTruck(TruckID truckID, out TruckSwitchReport report)
        {
            if (!_playerData.TrySwitchVehicle(truckID, out report)) return false;
            else
            {
                _playerTruck = ShowTruck(truckID, _playerController.ActiveVehicle.FormVirtualPoint());
                _playerController.ChangeActiveVehicle(_playerTruck);
                return true;
            }
        }
    }
}
